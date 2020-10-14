using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WebApplicationAPI.Domain.Identity;

namespace WebApplicationAPI.Data.EntityTypesConfigurations {
    class IdentityEntitiesConfiguration : IEntityTypeConfiguration<User>, IEntityTypeConfiguration<Role>,
            IEntityTypeConfiguration<IdentityUserClaim<Guid>>, IEntityTypeConfiguration<UserRole>,
            IEntityTypeConfiguration<IdentityUserLogin<Guid>>, IEntityTypeConfiguration<IdentityRoleClaim<Guid>>,
            IEntityTypeConfiguration<IdentityUserToken<Guid>> {

        const string SCHEMA_NAME = "Identity";

        void IEntityTypeConfiguration<User>.Configure(EntityTypeBuilder<User> builder) {
            builder.ToTable("Users", SCHEMA_NAME);
            builder.Property(user => user.Gender)
                   .HasMaxLength(16);
        }

        void IEntityTypeConfiguration<Role>.Configure(EntityTypeBuilder<Role> builder) {
            builder.ToTable("Roles", SCHEMA_NAME);
        }

        void IEntityTypeConfiguration<IdentityUserClaim<Guid>>.Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder) {
            builder.ToTable("UserClaims", SCHEMA_NAME);
        }

        void IEntityTypeConfiguration<UserRole>.Configure(EntityTypeBuilder<UserRole> builder) {
            builder.ToTable("UserRoles", SCHEMA_NAME);
            builder.HasKey(userRole => new { userRole.UserId, userRole.RoleId });
            builder.HasOne(userRole => userRole.Role)
                   .WithMany(role => role.UserRoles)
                   .HasForeignKey(userRole => userRole.RoleId);
            builder.HasOne(userRole => userRole.User)
                   .WithMany(role => role.UserRoles)
                   .HasForeignKey(userRole => userRole.UserId);
        }

        void IEntityTypeConfiguration<IdentityUserLogin<Guid>>.Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder) {
            builder.ToTable("UserLogins", SCHEMA_NAME);
        }

        void IEntityTypeConfiguration<IdentityRoleClaim<Guid>>.Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder) {
            builder.ToTable("RoleClaims", SCHEMA_NAME);
        }

        void IEntityTypeConfiguration<IdentityUserToken<Guid>>.Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder) {
            builder.ToTable("UserTokens", SCHEMA_NAME);
        }
    }
}
