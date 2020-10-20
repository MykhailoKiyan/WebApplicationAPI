using System;
using System.Collections.Generic;
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
using WebApplicationAPI.Domain.Identity;
using WebApplicationAPI.Options;

namespace WebApplicationAPI.Services {
    public class IdentityService : IIdentityService {
        private readonly UserManager<User> userManager;

        private readonly JwtSettings jwtSettings;

        private readonly TokenValidationParameters tokenValidationParameters;

        private readonly DataContext dbContext;

        private readonly RoleManager<Role> roleManager;

        public IdentityService(
                UserManager<User> userManager,
                JwtSettings jwtSettings,
                TokenValidationParameters tokenValidationParameters,
                DataContext context,
                RoleManager<Role> roleManager) {

            this.userManager = userManager;
            this.jwtSettings = jwtSettings;
            this.tokenValidationParameters = tokenValidationParameters;
            this.dbContext = context;
            this.roleManager = roleManager;
        }

        public async Task<AuthenticationResult> RegisterAsync(
                string email,
                string password) {

            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null) return new AuthenticationResult { Errors = new[] { "User with this email address already exists" } };
            var newUser = new User { Id = Guid.NewGuid(), Email = email, UserName = email };
            var createdUser = await userManager.CreateAsync(newUser, password);
            if (!createdUser.Succeeded) return new AuthenticationResult { Errors = createdUser.Errors.Select(x => x.Description) };
            return await this.GenerateAuthenticationResultForUserAsync(newUser);
        }

        public async Task<AuthenticationResult> LoginAsync(
                string email,
                string password) {

            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return new AuthenticationResult { Errors = new[] { "User does not exist" } };
            var userHasValidPassword = await userManager.CheckPasswordAsync(user, password);
            if (!userHasValidPassword) return new AuthenticationResult { Errors = new[] { "User/password combination is wrong" } };
            return await this.GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(
                string token,
                string refreshToken) {

            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken == null) return new AuthenticationResult { Errors = new[] { "Invalid Token" } };
            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);
            if (expiryDateTimeUtc > DateTime.UtcNow) return new AuthenticationResult { Errors = new[] { "This token hasn't expired yet" } };
            var jti = validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = await dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
            if (storedRefreshToken == null) return new AuthenticationResult { Errors = new[] { "This refresh token does not exist" } };
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate) return new AuthenticationResult { Errors = new[] { "This refresh token has expired" } };
            if (storedRefreshToken.Invalidated) return new AuthenticationResult { Errors = new[] { "This refresh token has been invalidated" } };
            if (storedRefreshToken.Used) return new AuthenticationResult { Errors = new[] { "This refresh token has been used" } };
            if (storedRefreshToken.JwtId != jti) return new AuthenticationResult { Errors = new[] { "This refresh token does not match this JWT" } };
            storedRefreshToken.Used = true;
            dbContext.RefreshTokens.Update(storedRefreshToken);
            await dbContext.SaveChangesAsync();
            var user = await userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return await this.GenerateAuthenticationResultForUserAsync(user);
        }

        private ClaimsPrincipal? GetPrincipalFromToken(string token) {
            var tokenHandler = new JwtSecurityTokenHandler();
            try {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!this.IsJwtWithValidSecurityAlgorithm(validatedToken)) return null;
                return principal;
            } catch {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken) {
            return (validatedToken is JwtSecurityToken jwtSecurityToken)
                   && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(User user) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id.ToString())
            };
            var userClaims = await this.userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            var userRoles = await this.userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles) {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await this.roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await this.roleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims) {
                    if (claims.Contains(roleClaim)) continue;
                    claims.Add(roleClaim);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = new RefreshToken {
                Token = Guid.NewGuid().ToString().ToLowerInvariant(),
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };
            await dbContext.RefreshTokens.AddAsync(refreshToken);
            await dbContext.SaveChangesAsync();
            return new AuthenticationResult {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }
    }
}
