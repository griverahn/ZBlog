using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBlogAPI.Models;

namespace ZBlogAPI.DataContext.Maps
{
    public class CommentMap : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("int")
                .IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Description).HasColumnName("Description").HasColumnType("varchar").HasMaxLength(250)
                .IsRequired();
            builder.Property(x => x.UserId).HasColumnName("UserId").HasColumnType("nvarchar").HasMaxLength(450)
                   .IsRequired();
            builder.Property(x => x.PostId).HasColumnName("PostId").HasColumnType("int").IsRequired();

            builder.HasOne(x => x.Post).WithMany(c => c.Comments).HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(x => x.User).WithMany(c => c.Comments).HasForeignKey(x => x.UserId);
        }
    }
}
