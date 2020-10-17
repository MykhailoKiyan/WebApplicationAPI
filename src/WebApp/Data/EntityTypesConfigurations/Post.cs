using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplicationAPI.Data.EntityTypesConfigurations {
    internal class Post : IEntityTypeConfiguration<Domain.Post> {
        void IEntityTypeConfiguration<Domain.Post>.Configure(EntityTypeBuilder<Domain.Post> builder) {
            builder.ToTable("Posts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd();
            builder.Property(p => p.Name)
                   .HasMaxLength(450)
                   .IsRequired();
            builder.Property(p => p.UserId)
                   .IsRequired(false);
            builder.HasOne(p => p.User)
                   .WithMany()
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
