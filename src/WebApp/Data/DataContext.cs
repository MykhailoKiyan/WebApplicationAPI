using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using WebApplicationAPI.Data.EntityTypesConfigurations;
using WebApplicationAPI.Domain;

namespace WebApplicationAPI.Data {
    public class DataContext : IdentityDbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            IdentityConfigure(builder);
            builder.Ignore<Post>();
            builder.Ignore<Tag>();
            builder.Ignore<PostTag>();
            builder.Ignore<RefreshToken>();
            //builder.Entity<PostTag>().Ignore(xx => xx.Post).HasKey(x => new { x.PostId, x.TagName });
        }

        void IdentityConfigure(ModelBuilder builder) {
            var configuration = new IdentityEntitiesConfiguration();
            builder.ApplyConfiguration<IdentityUser>(configuration);
            builder.ApplyConfiguration<IdentityRole>(configuration);
            builder.ApplyConfiguration<IdentityUserClaim<string>>(configuration);
            builder.ApplyConfiguration<IdentityUserRole<string>>(configuration);
            builder.ApplyConfiguration<IdentityUserLogin<string>>(configuration);
            builder.ApplyConfiguration<IdentityRoleClaim<string>>(configuration);
            builder.ApplyConfiguration<IdentityUserToken<string>>(configuration);
        }
    }
}
