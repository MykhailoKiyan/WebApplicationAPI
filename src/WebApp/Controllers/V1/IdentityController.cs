using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Requests;
using WebApplicationAPI.Contracts.V1.Responses;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Controllers.V1 {
    public class IdentityController : Controller {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService) {
            this.identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request) {
            if (!this.ModelState.IsValid) return this.BadRequest(new AuthFailedResponse {
                Errors = this.ModelState.Values.SelectMany(mse => mse.Errors.Select(e => e.ErrorMessage)) });
            var authResponse = await identityService.RegisterAsync(request.Email, request.Password);
            if (!authResponse.Success) return this.BadRequest(new AuthFailedResponse { Errors = authResponse.Errors });
            return this.Ok(new AuthSuccessResponse { Token = authResponse.Token, RefreshToken = authResponse.RefreshToken });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request) {
            var authResponse = await identityService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Success) return this.BadRequest(new AuthFailedResponse { Errors = authResponse.Errors });
            return this.Ok(new AuthSuccessResponse { Token = authResponse.Token, RefreshToken = authResponse.RefreshToken });
        }

        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request) {
            var authResponse = await identityService.RefreshTokenAsync(request.Token, request.RefreshToken);
            if (!authResponse.Success) return this.BadRequest(new AuthFailedResponse { Errors = authResponse.Errors });
            return this.Ok(new AuthSuccessResponse { Token = authResponse.Token, RefreshToken = authResponse.RefreshToken });
        }
    }
}
