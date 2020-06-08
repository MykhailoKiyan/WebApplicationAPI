using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Requests;
using WebApplicationAPI.Contracts.V1.Responses;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Controllers.V1
{
    public class IdentityController : Controller {
      private readonly IIdentityService identityService;
      public IdentityController(IIdentityService identityService) {
        this.identityService = identityService;
      }

      [HttpPost(ApiRoutes.Identity.Register)]
      public async Task<ActionResult> Register ([FromBody] UserRegistrationRequest request) {
        var authResponse = await this.identityService.RegisterAsync(request.Email, request.Password);
        if (!authResponse.Success) return this.BadRequest(new AuthFailResponse { Errors = authResponse.Errors });
        else return this.Ok(new AuthSuccessResponse { Token = authResponse.Token });
      }
    }
}
