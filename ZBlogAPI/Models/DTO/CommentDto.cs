namespace ZBlogAPI.Models.DTO
{
    public class CommentDto
    {
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
