using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApplicationAPI.Domain.Identity {
    /// <summary>
    /// Represents a role of the Application.
    /// </summary>
    public class Role : IdentityRole<Guid> {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
