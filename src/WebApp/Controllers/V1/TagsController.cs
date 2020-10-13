using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Controllers.V1 {
    public class TagsController : Controller {
        private readonly IPostService _postService;

        public TagsController(IPostService postService) {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll() {
            return Ok(await _postService.GetAllTagsAsync());
        }
    }
}
