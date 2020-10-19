using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebApplicationAPI.Domain;

namespace WebApplicationAPI.Services {
    public interface IPostService {
        Task<List<Post>> GetPostsAsync();

        Task<bool> CreatePostAsync(Post post);

        Task<Post> GetPostByIdAsync(Guid postId);

        Task<bool> UpdatePostAsync(Post postToUpdate);

        Task<bool> DeletePostAsync(Guid postId);

        Task<bool> UserOwnsPostAsync(Guid postId, Guid userId);

        Task<List<Tag>> GetAllTagsAsync();
    }
}
