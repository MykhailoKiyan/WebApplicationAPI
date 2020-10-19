using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Controllers.V1 {
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : Controller {
        private readonly IPostService postService;

        public TagsController(IPostService postService) {
            this.postService = postService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        [Authorize(Policy = "TagViewer")]
        public async Task<IActionResult> GetAll() {
            return this.Ok(await postService.GetAllTagsAsync());
        }
    }
}
