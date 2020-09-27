using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPI.Contracts.V1.Requests {
    public class UserRegistrationRequest {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
