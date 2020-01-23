using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using WebApplicationAPI.Domain;
using WebApplicationAPI.ExtensionMethods;
using WebApplicationAPI.Contracts.V1;

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
    }
}
