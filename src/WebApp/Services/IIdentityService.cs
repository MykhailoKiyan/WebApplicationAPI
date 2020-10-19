using System.Threading.Tasks;

using WebApplicationAPI.Domain;

namespace WebApplicationAPI.Services {
    public interface IIdentityService {
        Task<AuthenticationResult> RegisterAsync(string email, string password);

        Task<AuthenticationResult> LoginAsync(string email, string password);

        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
