using System;
using Microsoft.AspNetCore.Identity;

namespace WebApplicationAPI.Domain.Identity {
    /// <summary>
    /// A join entity that associates users and roles of the Application.
    /// </summary>
    public class UserRole : IdentityUserRole<Guid> {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
