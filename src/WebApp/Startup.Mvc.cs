using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using WebApplicationAPI.Authorization;
using WebApplicationAPI.Filters;
using WebApplicationAPI.Services;

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

            services.AddSingleton<IUriService>(provider => {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
                return new UriService(absoluteUri);
            });
        }
    }
}
