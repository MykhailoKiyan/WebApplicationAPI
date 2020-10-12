using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using WebApplicationAPI.Domain;
using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Responses;
using WebApplicationAPI.Contracts.V1.Requests;
using WebApplicationAPI.Services;
using WebApplicationAPI.ExtensionMethods;

namespace WebApplicationAPI.Controllers.V1 {
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller {
        private readonly IPostService postService;

        public PostsController(
                IPostService postService
        ) {
            this.postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll() {
            List<Post> posts = await this.postService.GetPostsAsync();
            return this.Ok(posts);
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get(
                [FromRoute] Guid postId
        ) {
            Post post = await this.postService.GetPostByIdAsync(postId);
            if (post == null) return this.NotFound();
            else return this.Ok(post);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create(
                [FromBody] PostCreateRequest postRequest
        ) {
            var newPostId = Guid.NewGuid();
            var post = new Post {
                Name = postRequest.Name,
                UserId = this.HttpContext.GetUserId(),
                Tags = postRequest.Tags?.Select(x => new PostTag { PostId = newPostId, TagName = x }).ToList()
            };
            await this.postService.CreatePostAsync(post);
            string baseUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}";
            string locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());
            var response = new PostResponse { Id = post.Id };
            return this.Created(locationUri, response);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update(
                [FromRoute] Guid postId,
                [FromBody] PostUpdateRequest request
        ) {
            string userId = HttpContext.GetUserId();
            bool isUserOwnsThePost = await this.postService.IsUserOwnsPostAsync(postId, userId);

            if (!isUserOwnsThePost) return this.BadRequest(new { errors = "You do not own this post" });

            var post = await this.postService.GetPostByIdAsync(postId);
            post.Name = request.Name;
            bool hasUpdated = await this.postService.UpdatePostAsync(post);

            if (hasUpdated) return this.Ok(post);
            else return this.NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete(
                [FromRoute] Guid postId
        ) {
            string userId = this.HttpContext.GetUserId();
            bool isUserOwnsThePost = await this.postService.IsUserOwnsPostAsync(postId, userId);

            if (!isUserOwnsThePost) return this.BadRequest(new { errors = "You do not own this post" });

            bool hasDeleted = await this.postService.DeletePostAsync(postId);

            if (hasDeleted) return this.NoContent();
            else return this.NotFound();
        }
    }
}
