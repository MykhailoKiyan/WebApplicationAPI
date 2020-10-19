using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Requests;
using WebApplicationAPI.Contracts.V1.Responses;
using WebApplicationAPI.Domain;
using WebApplicationAPI.ExtensionMethods;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Controllers.V1 {
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller {
        private readonly IPostService postService;

        public PostsController(IPostService postService) {
            this.postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll() {
            return this.Ok(await postService.GetPostsAsync());
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] PostUpdateRequest request) {
            var userId = this.HttpContext.GetUserId();
            if (!userId.HasValue) return this.BadRequest("The User Id is epsent");
            var userOwnsPost = await postService.UserOwnsPostAsync(postId, userId.Value);
            if (!userOwnsPost) return this.BadRequest(new { error = "You do not own this post" });
            var post = await postService.GetPostByIdAsync(postId);
            post.Name = request.Name;
            var updated = await postService.UpdatePostAsync(post);
            if (updated) return this.Ok(post);
            return this.NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId) {
            var userId = this.HttpContext.GetUserId();
            if (!userId.HasValue) return this.BadRequest("The User Id is epsent");
            var userOwnsPost = await postService.UserOwnsPostAsync(postId, userId.Value);
            if (!userOwnsPost) return this.BadRequest(new { error = "You do not own this post" });
            var deleted = await postService.DeletePostAsync(postId);
            if (deleted) return this.NoContent();
            return this.NotFound();
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId) {
            var post = await postService.GetPostByIdAsync(postId);
            if (post == null) return this.NotFound();
            return this.Ok(post);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] PostCreateRequest postRequest) {
            var userId = this.HttpContext.GetUserId();
            if (!userId.HasValue) return this.BadRequest("The User Id is epsent");
            var newPostId = Guid.NewGuid();
            var post = new Post { Id = newPostId, Name = postRequest.Name, UserId = userId.Value,
                Tags = postRequest.Tags.Select(t => new PostTag { PostId = newPostId, TagName = t }).ToList()
            };
            await postService.CreatePostAsync(post);
            var baseUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());
            var response = new PostResponse { Id = post.Id };
            return this.Created(locationUri, response);
        }
    }
}
