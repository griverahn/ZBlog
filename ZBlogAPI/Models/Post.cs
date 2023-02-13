namespace ZBlogAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime PublishingDate { get; set; }
        public User User { get; set; } = default!;
        public string ApprovalComments { get; set; } = string.Empty;

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
