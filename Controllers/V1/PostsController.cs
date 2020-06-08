using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebApplicationAPI.Domain;
using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Responses;
using WebApplicationAPI.Contracts.V1.Requests;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Controllers.V1 {
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
      return Ok(posts);
    }

    [HttpGet(ApiRoutes.Posts.Get)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid postId
    ) {
      Post post = await this.postService.GetPostByIdAsync(postId);
      if (post == null) return this.NotFound();
      else return Ok(post);
    }

    [HttpPost(ApiRoutes.Posts.Create)]
    public async Task<IActionResult> Create(
        [FromBody] PostCreateRequest postRequest
    ) {
      var post = new Post { Name = postRequest.Name };
      await this.postService.CreatePostAsync(post);
      string baseUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}";
      string locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());
      var response = new PostResponse { Id = post.Id };
      return this.Created(locationUri, response);
    }

    [HttpPut(ApiRoutes.Posts.Update)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid              postId,
        [FromBody]  PostUpdateRequest request
    ) {
      var post = new Post { Id = postId, Name = request.Name };
      bool hasUpdated = await this.postService.UpdatePostAsync(post);
      if (hasUpdated) return this.Ok(post);
      else return this.NotFound();
    }

    [HttpDelete(ApiRoutes.Posts.Delete)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid postId
    ) {
      bool hasDeleted = await this.postService.DeletePostAsync(postId);
      if (hasDeleted) return this.NoContent();
      else return this.NotFound();
    }
  }
}
