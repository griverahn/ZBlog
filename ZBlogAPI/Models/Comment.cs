using System.ComponentModel.DataAnnotations;

namespace ZBlogAPI.Models
{
    public class Comment
    {
     
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int PostId { get; set; } 

        public virtual Post Post { get; set; } = default!;
        public virtual User User { get; set; } = default!;
    }
}
