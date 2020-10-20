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

        public async Task<List<Post>> GetPostsAsync() {
            return await dataContext.Posts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId) {
            return await dataContext.Posts
                .Include(x => x.Tags)
                .SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<bool> CreatePostAsync(Post post) {
            post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            await this.AddNewTags(post);
            await dataContext.Posts.AddAsync(post);
            var created = await dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate) {
            postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            await this.AddNewTags(postToUpdate);
            dataContext.Posts.Update(postToUpdate);
            var updated = await dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId) {
            var post = await this.GetPostByIdAsync(postId);
            if (post == null) return false;
            dataContext.Posts.Remove(post);
            var deleted = await dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, Guid userId) {
            var post = await dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);
            if (post == null) return false;
            if (post.UserId != userId) return false;
            return true;
        }

        public async Task<List<Tag>> GetAllTagsAsync() {
            return await dataContext.Tags.AsNoTracking().ToListAsync();
        }

        private async Task AddNewTags(Post post) {
            foreach (var tag in post.Tags) {
                var existingTag = await dataContext.Tags.SingleOrDefaultAsync(t => t.Name == tag.TagName);
                if (existingTag != null) continue;
                await dataContext.Tags.AddAsync(new Tag { Name = tag.TagName, CreatedOn = DateTime.UtcNow, CreatorId = post.UserId });
            }
        }

        public async Task<bool> CreateTagAsync(Tag tag) {
            tag.Name = tag.Name.ToLower();
            var existingTag = this.dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(t => t.Name == tag.Name);
            if (existingTag != null) return true;
            await this.dataContext.Tags.AddAsync(tag);
            var createdCount = await this.dataContext.SaveChangesAsync();
            return createdCount > 0;
        }

        public Task<Tag> GetTagByNameAsync(string tagName) {
            return this.dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(p => p.Name == tagName);
        }

        public async Task<bool> DeleteTagAsync(string tagName) {
            tagName = tagName.ToLower();
            var tag = await this.dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName);
            if (tag == null) return true;
            var postTags = await this.dataContext.PostTags.Where(x => x.TagName == tagName).ToListAsync();
            this.dataContext.PostTags.RemoveRange(postTags);
            this.dataContext.Tags.Remove(tag);
            return await this.dataContext.SaveChangesAsync() > 0;
        }
    }
}
