using System;
using System.Collections.Generic;

using WebApplicationAPI.Domain;

namespace WebApplicationAPI.Services {
  public interface IPostService {
    List<Post> GetPosts();
    Post GetPostById(Guid postId);
    bool UpdatePost(Post post);
    bool DeletePost(Guid postId);
  }
}