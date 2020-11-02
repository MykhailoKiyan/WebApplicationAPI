using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

using WebApplicationAPI.Authorization;
using WebApplicationAPI.Filters;

namespace WebApplicationAPI {
    public partial class Startup {
        partial void MvcConfigureServices(IServiceCollection services) {
            services
                .AddMvc(options => {
                    options.EnableEndpointRouting = false;
                    options.Filters.Add<ValidationFilter>();
                })
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddControllers()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }
    }
}
