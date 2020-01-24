using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace WebApplicationAPI.Installers {
    public class MvcInstaller : IInstaller {
        public void InstallServices(
            IServiceCollection services
          , IConfiguration configuration
        ) {
          services.AddMvc();

          // Register the Swagger generator, defining 1 or more Swagger documents
          services.AddSwaggerGen( setupAction =>
            setupAction.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }));
        }
    }
}