using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ZBlogAPI.Models
{
    public class User : IdentityUser
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
