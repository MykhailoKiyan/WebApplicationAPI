using Microsoft.Extensions.DependencyInjection;

using WebApplicationAPI.Data;
using WebApplicationAPI.HealthChecks;

namespace WebApplicationAPI {
    public partial class Startup {
        partial void HelthChecksConfigureServices(IServiceCollection services) {
            services
                .AddHealthChecks()
                .AddDbContextCheck<DataContext>()
                .AddCheck<RedisHealthCheck>("Redis");
        }
    }
}
