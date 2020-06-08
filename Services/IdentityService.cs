using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApplicationAPI.Domain;
using WebApplicationAPI.Options;

namespace WebApplicationAPI.Services {
  public class IdentityService : IIdentityService {
    private readonly UserManager<IdentityUser> userManager;
    private readonly JwtSettings jwtSettings;

    public IdentityService(
        UserManager<IdentityUser> userManager,
        JwtSettings               jwtSettings
    ) {
      this.userManager = userManager;
      this.jwtSettings = jwtSettings;
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password) {
      IdentityUser? user = await this.userManager.FindByEmailAsync(email);
      if (user == null) return new AuthenticationResult {
        Success = false,
        Errors = new[] { "User with this email does not exist" }
      };

      bool isValidPassword = await this.userManager.CheckPasswordAsync(user, password);
      if (!isValidPassword) return new AuthenticationResult {
        Success = false,
        Errors = new[] { "User/password combination is wrong" }
      };

      return GenerateAuthenticationResultForUser(user);
    }

    public async Task<AuthenticationResult> RegisterAsync(string email, string password) {
      IdentityUser? existingUser = await this.userManager.FindByEmailAsync(email);
      if (existingUser != null) return new AuthenticationResult {
        Success = false,
        Errors = new[] { "User with this email address already exists" }
      };

      var newUser = new IdentityUser { Email = email, UserName = email };
      IdentityResult identityResult = await this.userManager.CreateAsync(newUser, password);
      if(!identityResult.Succeeded) return new AuthenticationResult {
        Errors = identityResult.Errors.Select(e => e.Description)
      };

      return GenerateAuthenticationResultForUser(newUser);
    }

    private AuthenticationResult GenerateAuthenticationResultForUser(IdentityUser user) {
      var tokenHandler = new JwtSecurityTokenHandler();
      byte[] key = Encoding.ASCII.GetBytes(this.jwtSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor {
        Subject = new ClaimsIdentity(new[] {
          new Claim(JwtRegisteredClaimNames.Sub, user.Email),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
          new Claim(JwtRegisteredClaimNames.Email, user.Email),
          new Claim("id", user.Id)
        }),
        Expires = DateTime.UtcNow.AddHours(2),
        SigningCredentials =
          new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return new AuthenticationResult {
        Success = true,
        Token = tokenHandler.WriteToken(token)
      };
    }
  }
}
