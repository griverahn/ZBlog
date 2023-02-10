using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ZBlogAPI.Models;

namespace ZBlogAPI.DataContext.Maps
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("int")
                .IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName("Title").HasColumnType("varchar").HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Description).HasColumnName("Description").HasColumnType("varchar")
                .HasMaxLength(500).IsRequired();
            builder.Property(x => x.Status).HasColumnName("Status").HasColumnType("varchar")
                .HasMaxLength(50).IsRequired();
            builder.Property(x => x.PublishingDate).HasColumnName("PublishingDate").IsRequired();
            builder.Property(x => x.UserId).HasColumnName("UserId").HasColumnType("nvarchar").HasMaxLength(450)
                .IsRequired();

            builder.HasOne(x=> x.User).WithMany(c=> c.Posts).HasForeignKey(x => x.UserId);

        }
    }
}
