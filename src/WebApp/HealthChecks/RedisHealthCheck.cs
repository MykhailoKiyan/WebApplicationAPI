using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace WebApplicationAPI.HealthChecks {
    public class RedisHealthCheck : IHealthCheck {
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public RedisHealthCheck(IConnectionMultiplexer connectionMultiplexer) {
            this.connectionMultiplexer = connectionMultiplexer;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) {
            try {
                var database = this.connectionMultiplexer.GetDatabase();
                database.StringGet("health");
                return Task.FromResult(HealthCheckResult.Healthy());
            } catch (Exception ex) {
                return Task.FromResult(HealthCheckResult.Unhealthy(ex.Message));
            }
        }
    }
}
