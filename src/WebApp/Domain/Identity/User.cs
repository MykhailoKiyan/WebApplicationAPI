using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApplicationAPI.Domain.Identity {
    /// <summary>
    /// Represents the user of the Application.
    /// </summary>
    public class User : IdentityUser<Guid> {
        public string Gender { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
