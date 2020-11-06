using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationAPI.Contracts.HealthChecks {
    public class HealthCheckResponse {
        public string Statuc { get; set; }

        public IEnumerable<HealthCheck> Checks { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
