using Microsoft.AspNetCore.Authorization;

namespace WebApplicationAPI.Authorization {
    public class WorksForCompanyRequirement : IAuthorizationRequirement {
        public string DomainName { get; }

        public WorksForCompanyRequirement(string domainName) {
            this.DomainName = domainName;
        }
    }
}
