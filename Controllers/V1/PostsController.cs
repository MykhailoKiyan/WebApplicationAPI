using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using WebApplicationAPI.Domain;
using WebApplicationAPI.ExtensionMethods;
using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Responses;
using WebApplicationAPI.Contracts.V1.Requests;

namespace WebApplicationAPI.Controllers.V1 {
  public class PostsController : Controller {
    private List<Post> posts;

    public PostsController() {
      this.posts = new List<Post>();
      Enumerable.Range(0, 5).ForEach(item => this.posts.Add(new Post { Id = Guid.NewGuid().ToString() }));
    }

    [HttpGet(ApiRoutes.Posts.GetAll)]
    public IActionResult GetAll() { 
      return Ok(this.posts);
    }

    [HttpPost(ApiRoutes.Posts.Create)]
    public IActionResult Create(
      [FromBody] PostCreateRequest postRequest
    ) {
      var post = new Post { Id = postRequest.Id };
      if (string.IsNullOrEmpty(post.Id)) {
        post.Id = Guid.NewGuid().ToString();
      }
      this.posts.Add(post);
      var baseUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}";
      var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id);
      var response = new PostResponse { Id = post.Id };
      return this.Created(locationUri, response);
    }
  }
}
