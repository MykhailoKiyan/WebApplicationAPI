using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Controllers.V1 {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : ControllerBase {
        private readonly IPostService postService;

        public TagsController(IPostService postService) {
            this.postService = postService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll() {
            return Ok(await this.postService.GetAllTagsAsync());
        }
    }
}
