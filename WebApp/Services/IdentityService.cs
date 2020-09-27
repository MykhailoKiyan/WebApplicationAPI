using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using WebApplicationAPI.Data;
using WebApplicationAPI.Domain;
using WebApplicationAPI.Options;

namespace WebApplicationAPI.Services {
    public class IdentityService : IIdentityService {
        private readonly UserManager<IdentityUser> userManager;

        private readonly JwtSettings jwtSettings;

        private readonly TokenValidationParameters tokenValidationParameters;

        private readonly DataContext dbContext;

        public IdentityService(
                UserManager<IdentityUser> userManager,
                JwtSettings jwtSettings,
                TokenValidationParameters tokenValidationParameters,
                DataContext dbContext
        ) {
            this.userManager = userManager;
            this.jwtSettings = jwtSettings;
            this.tokenValidationParameters = tokenValidationParameters;
            this.dbContext = dbContext;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password) {
            IdentityUser? user = await this.userManager.FindByEmailAsync(email);
            if (user == null) return new AuthenticationResult {
                Success = false,
                Errors = new[] { "User with this email does not exist" }
            };

            bool isValidPassword = await this.userManager.CheckPasswordAsync(user, password);
            return !isValidPassword
                ? new AuthenticationResult {
                    Success = false,
                    Errors = new[] { "User/password combination is wrong" }
                }
                : await this.GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken) {
            ClaimsPrincipal? validatedToken = this.GetClaimsPrincipal(token);

            if (validatedToken == null) return new AuthenticationResult {
                Success = false,
                Errors = new[] { "Invalid token" }
            };

            long expiryDateUnix = long.Parse(validatedToken.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.Exp)
                .Value);
            DateTime expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
              .AddSeconds(expiryDateUnix);

            if (expiryDateUtc > DateTime.UtcNow) return new AuthenticationResult {
                Success = false,
                Errors = new[] { "The token has not expired yet" }
            };

            string jti = validatedToken.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.Jti)
                .Value;
            RefreshToken storedToken = await this.dbContext.RefreshTokens
                .SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedToken == null) return new AuthenticationResult {
                Success = false,
                Errors = new[] { "The refresh token does not exist" }
            };

            if (DateTime.UtcNow > storedToken.ExpiryDate) return new AuthenticationResult {
                Success = false,
                Errors = new[] { "The refresh token has expired" }
            };

            if (storedToken.Invalidated) return new AuthenticationResult {
                Success = false,
                Errors = new[] { "The refresh token is invalidated" }
            };

            if (storedToken.Used) return new AuthenticationResult {
                Success = false,
                Errors = new[] { "The refresh token has been used" }
            };

            if (storedToken.JwtId != jti) return new AuthenticationResult {
                Success = false,
                Errors = new[] { "The refresh token does not match the JWT" }
            };

            storedToken.Used = true;
            this.dbContext.RefreshTokens.Update(storedToken);
            await this.dbContext.SaveChangesAsync();

            IdentityUser user = await this.userManager
              .FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id")
              .Value);
            return await this.GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password) {
            IdentityUser? existingUser = await this.userManager.FindByEmailAsync(email);

            if (existingUser != null) return new AuthenticationResult {
                Success = false,
                Errors = new[] { "User with this email address already exists" }
            };

            var newUser = new IdentityUser { Email = email, UserName = email };
            IdentityResult identityResult = await this.userManager.CreateAsync(newUser, password);

            return !identityResult.Succeeded
                ? new AuthenticationResult {
                    Errors = identityResult.Errors.Select(e => e.Description)
                }
                : await this.GenerateAuthenticationResultForUserAsync(newUser);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user) {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(this.jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.Add(this.jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            DateTime utcDateNow = DateTime.UtcNow;
            var refreshToken = new RefreshToken {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = utcDateNow,
                ExpiryDate = utcDateNow.AddMonths(6)
            };
            await this.dbContext.RefreshTokens.AddAsync(refreshToken);
            await this.dbContext.SaveChangesAsync();
            return new AuthenticationResult {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        private ClaimsPrincipal? GetClaimsPrincipal(string token) {
            var tokenHandler = new JwtSecurityTokenHandler();
            try {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                    this.tokenValidationParameters, out SecurityToken? validatedToken);

                return !this.IsJwtWithSecurityAlgorithm(validatedToken) ? null : principal;
            } catch {
                return null;
            }
        }

        private bool IsJwtWithSecurityAlgorithm(SecurityToken validatedToken) {
            return validatedToken is JwtSecurityToken jwtSecurityToken  && jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
