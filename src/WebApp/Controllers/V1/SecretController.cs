using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Filters;

namespace WebApplicationAPI.Controllers.V1 {
    [ApiKeyAuth]
    public class SecretController : ControllerBase {
        [HttpGet("secret")]
        public IActionResult GetSecret() {
            return this.Ok("I have no secret");
        }
    }
}
