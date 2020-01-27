using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using WebApplicationAPI.Data;
using WebApplicationAPI.Domain;
using WebApplicationAPI.ExtensionMethods;

namespace WebApplicationAPI.Services {
  public class PostService : IPostService {
    private readonly DataContext dataContext;

    public PostService(DataContext dataContext) {
      this.dataContext = dataContext;
    }

    public async Task<Post> GetPostByIdAsync(Guid postId) {
      return await this.dataContext.Posts.SingleOrDefaultAsync(post => post.Id == postId);
    }

    public async Task<List<Post>> GetPostsAsync() {
      return await this.dataContext.Posts.ToListAsync();
    }

    public async Task<bool> CreatePostAsync(Post post) {
      await this.dataContext.AddAsync(post);
      int rowsCreated = await this.dataContext.SaveChangesAsync();
      if (rowsCreated == 1) {
        return true;
      } else {
        return false;
      }
    }

    public async Task<bool> UpdatePostAsync(Post post) {
      this.dataContext.Posts!.Update(post);
      int rowsUpdated = await this.dataContext.SaveChangesAsync();
      if (rowsUpdated == 1) {
        return true;
      } else {
        return false;
      }
    }

    public async Task<bool> DeletePostAsync(Guid postId) {
      Post post = await this.GetPostByIdAsync(postId);
      if (post == null) {
        return false;
      }
      this.dataContext.Posts!.Remove(post);
      int rowsDeleted = await this.dataContext.SaveChangesAsync();
      if (rowsDeleted == 1) {
        return true;
      } else {
        return false;
      }
    }
  }
}