using System;
using System.Linq;
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
    public IActionResult GetAll() { 
      return Ok(this.postService.GetPosts());
    }

    [HttpGet(ApiRoutes.Posts.Get)]
    public IActionResult Get(
        [FromRoute] Guid postId
    ) {
      Post post = this.postService.GetPostById(postId);
      if (post == null) {
        return this.NotFound();
      }
      return Ok(post);
    }

    [HttpPost(ApiRoutes.Posts.Create)]
    public IActionResult Create(
      [FromBody] PostCreateRequest postRequest
    ) {
      var post = new Post { Id = postRequest.Id };
      if (post.Id == Guid.Empty) {
        post.Id = Guid.NewGuid();
      }
      //this.posts.Add(post);
      var baseUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}";
      var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());
      var response = new PostResponse { Id = post.Id };
      return this.Created(locationUri, response);
    }

    [HttpPut(ApiRoutes.Posts.Update)]
    public IActionResult Update(
        [FromRoute] Guid              postId
      , [FromBody]  UpdatePostRequest request
    ) {
      Post post = new Post {
          Id = postId
        , Name = request.Name
      };

      bool isUpdated = this.postService.UpdatePost(post);
      if (isUpdated) {
        return this.Ok(post);
      }
      return this.NotFound();
    }
  }
}
