using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace WebApplicationAPI {
    public partial class Startup {
        partial void MvcConfigureServices(IServiceCollection services) {
            services.AddMvc(options => { options.EnableEndpointRouting = false; });

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Tweetbook API",
                    Version = "v1"
                });

                var security = new Dictionary<string, IEnumerable<string>> {
                    {
                        "Bearer", new string[0]
                    }
                };

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            services.AddAuthorization(options => {
                options.AddPolicy("TagViewer", builder => builder.RequireClaim("tags.view", "true"));
            });

            services.AddControllers()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }
    }
}
