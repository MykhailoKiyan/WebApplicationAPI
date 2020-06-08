using System.Threading.Tasks;

using WebApplicationAPI.Domain;

namespace WebApplicationAPI.Services {
  public interface IIdentityService {
    Task<AuthenticationResult> RegisterAsync(string email, string password);
  }
}
