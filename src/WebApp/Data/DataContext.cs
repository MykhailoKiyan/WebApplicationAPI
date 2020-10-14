using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using WebApplicationAPI.Domain.Identity;

namespace WebApplicationAPI.Data {
    public class DataContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole,
                               IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<User>(user => {
                user.ToTable("Users", "Identity");
                user.Property(user => user.Gender)
                    .HasMaxLength(16);
            });

            builder.Entity<Role>(role => {
                role.ToTable("Roles", "Identity");
            });

            builder.Entity<UserRole>(userRole => {
                userRole.ToTable("UserRoles", "Identity");
                userRole.HasKey(userRole => new { userRole.UserId, userRole.RoleId });
                userRole.HasOne(userRole => userRole.Role)
                        .WithMany(role => role.UserRoles)
                        .HasForeignKey(userRole => userRole.RoleId)
                        .IsRequired();
                userRole.HasOne(userRole => userRole.User)
                        .WithMany(role => role.UserRoles)
                        .HasForeignKey(userRole => userRole.UserId)
                        .IsRequired();
            });

            builder.Entity<IdentityUserClaim<Guid>>(userClaim => {
                userClaim.ToTable("UserClaims", "Identity");
            });

            builder.Entity<IdentityUserLogin<Guid>>(userLogin => {
                userLogin.ToTable("UserLogins", "Identity");
            });

            builder.Entity<IdentityUserToken<Guid>>(userToken => {
                userToken.ToTable("UserTokens", "Identity");
            });

            builder.Entity<IdentityRoleClaim<Guid>>(roleClaim => {
                roleClaim.ToTable("RoleClaims", "Identity");
            });
        }
    }
}
