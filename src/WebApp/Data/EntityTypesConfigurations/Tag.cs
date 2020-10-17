using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplicationAPI.Data.EntityTypesConfigurations {
    sealed class Tag : IEntityTypeConfiguration<Domain.Tag> {
        void IEntityTypeConfiguration<Domain.Tag>.Configure(EntityTypeBuilder<Domain.Tag> builder) {
            builder.ToTable("Tags");
            builder.HasKey(t => t.Name);
            builder.Property(t => t.Name)
                   .IsRequired()
                   .HasMaxLength(450);
            builder.Property(t => t.CreatorId)
                   .IsRequired(false);
            builder.Property(t => t.CreatedOn)
                   .IsRequired()
                   .HasDefaultValueSql("SYSUTCDATETIME()");
            builder.HasOne(p => p.CreatedBy)
                   .WithMany()
                   .HasForeignKey(p => p.CreatorId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
