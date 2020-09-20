using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;

using WebApplicationAPI.Options;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Installers {
  public class MvcInstaller : IInstaller {
    public void InstallServices(
        IServiceCollection services,
        IConfiguration configuration
    ) {
      var jwtSettings = new JwtSettings();
      configuration.Bind(nameof(jwtSettings), jwtSettings);
      services.AddSingleton(jwtSettings);

      services.AddScoped<IIdentityService, IdentityService>();

      services.AddMvc();

      var tokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true
      };

      services.AddSingleton(tokenValidationParameters);

      services
        .AddAuthentication(c => {
          c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          c.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
          c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(c => {
          c.SaveToken = true;
          c.TokenValidationParameters = tokenValidationParameters;
        });

      services
        .AddAuthentication();

      // Register the Swagger generator, defining 1 or more Swagger documents
      services.AddSwaggerGen(s => {
        s.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer",
          BearerFormat = "JWT",
          In = ParameterLocation.Header,
          Description = "JWT Authorization header using the Bearer scheme."
        });

        s.AddSecurityRequirement(new OpenApiSecurityRequirement {
          {
            new OpenApiSecurityScheme {
              Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
            },
            new string[] {}
          }
        });
      });
    }
  }
}
