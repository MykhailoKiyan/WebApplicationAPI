using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using WebApplicationAPI.Data.EntityTypesConfigurations;
using WebApplicationAPI.Domain;
using WebApplicationAPI.Domain.Identity;

namespace WebApplicationAPI.Data {
    public class DataContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole,
                               IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Domain.RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            this.IdentityConfigure(builder);
            builder.ApplyConfiguration<Domain.RefreshToken>((IEntityTypeConfiguration<Domain.RefreshToken>)new EntityTypesConfigurations.RefreshToken());
        }

        void IdentityConfigure(ModelBuilder builder) {
            var configuration = new Identities();
            builder.ApplyConfiguration<User>(configuration);
            builder.ApplyConfiguration<Role>(configuration);
            builder.ApplyConfiguration<IdentityUserClaim<Guid>>(configuration);
            builder.ApplyConfiguration<UserRole>(configuration);
            builder.ApplyConfiguration<IdentityUserLogin<Guid>>(configuration);
            builder.ApplyConfiguration<IdentityRoleClaim<Guid>>(configuration);
            builder.ApplyConfiguration<IdentityUserToken<Guid>>(configuration);
        }
    }
}
