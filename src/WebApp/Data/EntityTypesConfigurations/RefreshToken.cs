using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplicationAPI.Data.EntityTypesConfigurations {
    public class RefreshToken : IEntityTypeConfiguration<Domain.RefreshToken> {
        void IEntityTypeConfiguration<Domain.RefreshToken>.Configure(EntityTypeBuilder<Domain.RefreshToken> builder) {
            builder.ToTable("RefreshTokens", "Auth").HasKey(rt => rt.Token);
            builder.Property(rt => rt.Token).HasMaxLength(450).IsRequired().ValueGeneratedNever();
            builder.Property(rt => rt.JwtId).HasMaxLength(450).IsRequired();
            builder.Property(rt => rt.CreationDate).IsRequired();
            builder.Property(rt => rt.ExpiryDate).IsRequired();
            builder.Property(rt => rt.Used).IsRequired();
            builder.Property(rt => rt.Invalidated).IsRequired();
            builder.Property(rt => rt.UserId).IsRequired();
            builder.HasOne(rt => rt.User).WithMany().HasForeignKey(rt => rt.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
