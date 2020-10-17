using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplicationAPI.Data.EntityTypesConfigurations {
    internal class PostTag : IEntityTypeConfiguration<Domain.PostTag> {
        void IEntityTypeConfiguration<Domain.PostTag>.Configure(EntityTypeBuilder<Domain.PostTag> builder) {
            builder.ToTable("PostTags");
            builder.HasKey(pt => new { pt.PostId, pt.TagName });
            builder.Property(pt => pt.PostId)
                   .IsRequired();
            builder.Property(pt => pt.TagName)
                   .HasMaxLength(450)
                   .IsRequired();
            builder.HasOne(pt => pt.Tag)
                   .WithMany()
                   .HasForeignKey(pt => pt.TagName)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(pt => pt.Post)
                   .WithMany(p => p.Tags)
                   .HasForeignKey(pt => pt.PostId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
