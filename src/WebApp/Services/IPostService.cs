using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplicationAPI.Services {
    public interface IPostService {
        Task<List<Domain.Post>> GetPostsAsync();

        Task<bool> CreatePostAsync(Domain.Post post);

        Task<Domain.Post> GetPostByIdAsync(Guid postId);

        Task<bool> UpdatePostAsync(Domain.Post postToUpdate);

        Task<bool> DeletePostAsync(Guid postId);

        Task<bool> UserOwnsPostAsync(Guid postId, Guid userId);

        Task<List<Domain.Tag>> GetAllTagsAsync();

        Task<bool> CreateTagAsync(Domain.Tag tag);

        Task<Domain.Tag> GetTagByNameAsync(string tagName);

        Task<bool> DeleteTagAsync(string tagName);
    }
}
