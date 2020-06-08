using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebApplicationAPI.Domain;

namespace WebApplicationAPI.Services {
  public interface IPostService {
    Task<List<Post>> GetPostsAsync();
    Task<Post> GetPostByIdAsync(Guid postId);
    Task<bool> CreatePostAsync(Post post);
    Task<bool> UpdatePostAsync(Post post);
    Task<bool> DeletePostAsync(Guid postId);
    Task<bool> IsUserOwnsPostAsync(Guid postId, string userId);
  }
}
