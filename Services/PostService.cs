using System;
using System.Collections.Generic;
using System.Linq;
using WebApplicationAPI.Domain;
using WebApplicationAPI.ExtensionMethods;

namespace WebApplicationAPI.Services {
  public class PostService : IPostService {
    private readonly List<Post> posts;

    public PostService() {
      this.posts = new List<Post>();
      Enumerable
        .Range(0, 5)
        .ForEach(item =>
          this.posts.Add(new Post {
              Id = Guid.NewGuid()
            , Name = $"Post Name {item}"
          }));
    }


    public Post GetPostById(Guid postId) {
      return this.posts.SingleOrDefault(post => post.Id == postId);
    }

    public List<Post> GetPosts() {
      return this.posts;
    }
  }
}