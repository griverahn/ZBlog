using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZBlogAPI.DataContext.Maps;
using ZBlogAPI.Models;

namespace ZBlogAPI.DataContext
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        { 
            
            
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PostMap());
            builder.ApplyConfiguration(new CommentMap());

            base.OnModelCreating(builder);

        }
    }
}
