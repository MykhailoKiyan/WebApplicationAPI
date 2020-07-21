using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using Cosmonaut.Response;

using WebApplicationAPI.Domain;
/*
namespace WebApplicationAPI.Services {
  public class CosmosPostService : IPostService {
    private readonly ICosmosStore<CosmosPostDto> cosmosStore;
    public CosmosPostService(ICosmosStore<CosmosPostDto> cosmosStore) {
      this.cosmosStore = cosmosStore;
    }
    public async Task<bool> CreatePostAsync(Post post) {
      var cosmosPost = new CosmosPostDto { Id = Guid.NewGuid().ToString(), Name = post.Name };
      CosmosResponse<CosmosPostDto> response = await this.cosmosStore.AddAsync(cosmosPost);
      post.Id = Guid.Parse(cosmosPost.Id);
      return response.IsSuccess;
    }

    public async Task<bool> DeletePostAsync(Guid postId) {
      CosmosResponse<CosmosPostDto> response =
        await this.cosmosStore.RemoveByIdAsync(postId.ToString(), postId.ToString());
      return response.IsSuccess;
    }

    public async Task<Post?> GetPostByIdAsync(Guid postId) {
      CosmosPostDto cosmosPost = await this.cosmosStore.FindAsync(postId.ToString(), postId.ToString());
      return cosmosPost == null
        ? null
        : new Post { Id = postId, Name = cosmosPost.Name };
    }

    public async Task<List<Post>> GetPostsAsync() {
      var posts = await this.cosmosStore.Query().ToListAsync();
      return posts.Select(item => new Post { Id = Guid.Parse(item.Id), Name = item.Name }).ToList();
    }

    public async Task<bool> UpdatePostAsync(Post post) {
      CosmosPostDto cosmosPost = await this.cosmosStore.FindAsync(post.Id.ToString(), post.Id.ToString());
      cosmosPost.Name = post.Name;
      CosmosResponse<CosmosPostDto> response = await this.cosmosStore.UpdateAsync(cosmosPost);
      return response.IsSuccess;
    }
  }
}
*/
