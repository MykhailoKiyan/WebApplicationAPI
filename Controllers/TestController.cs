using Microsoft.AspNetCore.Mvc;

namespace WebApplicationAPI.Controllers {
    public class TestController : Controller {
        [HttpGet("api/user")]
        public IActionResult Get() {
            return this.Ok(new { name = "Nick" });
        }
    }
}
