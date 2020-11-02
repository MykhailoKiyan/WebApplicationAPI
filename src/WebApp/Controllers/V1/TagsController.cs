using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Requests;
using WebApplicationAPI.Contracts.V1.Responses;
using WebApplicationAPI.ExtensionMethods;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Controllers.V1 {
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : Controller {
        private readonly IPostService postService;
        private readonly IMapper mapper;

        public TagsController(IPostService postService, IMapper mapper) {
            this.postService = postService;
            this.mapper = mapper;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() {
            var tags = await postService.GetAllTagsAsync();
            return this.Ok(this.mapper.Map<List<TagResponse>>(tags));
        }

        [HttpGet(ApiRoutes.Tags.Get)]
        public async Task<IActionResult> Get([FromRoute] string tagName) {
            var tag = await this.postService.GetTagByNameAsync(tagName);
            if (tag == null) return this.NotFound();
            else return this.Ok(this.mapper.Map<TagResponse>(tag));
        }

        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> Create([FromBody] TagCreateRequest request) {
            var tag = new Domain.Tag {
                Name = request.TagName,
                CreatorId = this.HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            var isCreated = await this.postService.CreateTagAsync(tag);
            if (!isCreated) return this.BadRequest(new { error = "Unable to create tag." });
            var baseUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagName}", tag.Name);
            return this.Created(locationUri, this.mapper.Map<TagResponse>(tag));
        }

        [HttpDelete(ApiRoutes.Tags.Delete)]
        [Authorize(Policy = "MustWorkForCompany")]
        public async Task<IActionResult> Delete([FromRoute] string tagName) {
            var isDeleted = await this.postService.DeleteTagAsync(tagName);
            if (isDeleted) return this.NoContent();
            else return this.NotFound();
        }
    }
}
