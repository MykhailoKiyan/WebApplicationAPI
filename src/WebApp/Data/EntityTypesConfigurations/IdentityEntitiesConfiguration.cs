using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplicationAPI.Data.EntityTypesConfigurations {
    class IdentityEntitiesConfiguration : IEntityTypeConfiguration<IdentityUser>, IEntityTypeConfiguration<IdentityRole>,
            IEntityTypeConfiguration<IdentityUserClaim<string>>, IEntityTypeConfiguration<IdentityUserRole<string>>,
            IEntityTypeConfiguration<IdentityUserLogin<string>>, IEntityTypeConfiguration<IdentityRoleClaim<string>>,
            IEntityTypeConfiguration<IdentityUserToken<string>> {

        const string SCHEMA_NAME = "Identity";

        void IEntityTypeConfiguration<IdentityUser>.Configure(EntityTypeBuilder<IdentityUser> builder) {
            builder.ToTable("Users", SCHEMA_NAME);
        }

        void IEntityTypeConfiguration<IdentityRole>.Configure(EntityTypeBuilder<IdentityRole> builder) {
            builder.ToTable("Roles", SCHEMA_NAME);
        }

        void IEntityTypeConfiguration<IdentityUserClaim<string>>.Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder) {
            builder.ToTable("UserClaims", SCHEMA_NAME);
        }

        void IEntityTypeConfiguration<IdentityUserRole<string>>.Configure(EntityTypeBuilder<IdentityUserRole<string>> builder) {
            builder.ToTable("UserRoles", SCHEMA_NAME);
        }

        void IEntityTypeConfiguration<IdentityUserLogin<string>>.Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder) {
            builder.ToTable("UserLogins", SCHEMA_NAME);
        }

        void IEntityTypeConfiguration<IdentityRoleClaim<string>>.Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder) {
            builder.ToTable("RoleClaims", SCHEMA_NAME);
        }

        void IEntityTypeConfiguration<IdentityUserToken<string>>.Configure(EntityTypeBuilder<IdentityUserToken<string>> builder) {
            builder.ToTable("UserTokens", SCHEMA_NAME);
        }
    }
}
