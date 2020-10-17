using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationAPI.Data {
    public class DataContext : IdentityDbContext<Domain.Identity.User, Domain.Identity.Role, Guid, IdentityUserClaim<Guid>,
                               Domain.Identity.UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Domain.RefreshToken> RefreshTokens { get; set; }

        public DbSet<Domain.Post> Posts { get; set; }

        public DbSet<Domain.Tag> Tags { get; set; }

        public DbSet<Domain.PostTag> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            this.IdentityConfigure(builder);
            builder.ApplyConfiguration(new EntityTypesConfigurations.RefreshToken());
            builder.ApplyConfiguration(new EntityTypesConfigurations.Post());
            builder.ApplyConfiguration(new EntityTypesConfigurations.Tag());
            builder.ApplyConfiguration(new EntityTypesConfigurations.PostTag());
        }

        void IdentityConfigure(ModelBuilder builder) {
            var configuration = new EntityTypesConfigurations.Identities();
            builder.ApplyConfiguration<Domain.Identity.User>(configuration);
            builder.ApplyConfiguration<Domain.Identity.Role>(configuration);
            builder.ApplyConfiguration<IdentityUserClaim<Guid>>(configuration);
            builder.ApplyConfiguration<Domain.Identity.UserRole>(configuration);
            builder.ApplyConfiguration<IdentityUserLogin<Guid>>(configuration);
            builder.ApplyConfiguration<IdentityRoleClaim<Guid>>(configuration);
            builder.ApplyConfiguration<IdentityUserToken<Guid>>(configuration);
        }
    }
}
