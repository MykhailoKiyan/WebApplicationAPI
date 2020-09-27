using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    public async Task<List<Post>> GetPostsAsync() {
      return await this.dataContext.Posts
        .Include(x => x.Tags)
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<Post> GetPostByIdAsync(Guid postId) {
      return await this.dataContext.Posts
        .Include(x => x.Tags)
        .SingleOrDefaultAsync(post => post.Id == postId);
    }

    public async Task<bool> CreatePostAsync(Post post) {
      post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
      await AddNewTagsAsync(post);
      await this.dataContext.AddAsync(post);
      int rowsCreated = await this.dataContext.SaveChangesAsync();
      return rowsCreated > 0;
    }

    public async Task<bool> UpdatePostAsync(Post post) {
      post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
      await AddNewTagsAsync(post);
      this.dataContext.Posts!.Update(post);
      int rowsUpdated = await this.dataContext.SaveChangesAsync();
      return rowsUpdated > 0;
    }

    public async Task<bool> DeletePostAsync(Guid postId) {
      Post post = await this.GetPostByIdAsync(postId);
      if (post == null) return false;
      this.dataContext.Posts!.Remove(post);
      int rowsDeleted = await this.dataContext.SaveChangesAsync();
      return rowsDeleted > 0;
    }

    public async Task<bool> IsUserOwnsPostAsync(Guid postId, string userId) {
      Post post = await this.dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(p => p.Id == postId);
      if (post == null) return false;
      return post.UserId == userId;
    }

    public async Task<IEnumerable<Tag>> GetAllTagsAsync() {
      return await this.dataContext.Tags.AsNoTracking().ToListAsync();
    }

    private async Task AddNewTagsAsync(Post post) {
      foreach (var tag in post.Tags) {
        var existingTag = await this.dataContext.Tags.SingleOrDefaultAsync(x => x.Name == tag.TagName);
        if (existingTag != null) continue;
        await this.dataContext.Tags.AddAsync(new Tag {
          Name = tag.TagName,
          CreatedOn = DateTime.UtcNow,
          CreatorId = post.UserId
        });
      }
    }
  }
}
