namespace ZBlogAPI.Models.DTO
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime PublishingDate { get; set; }
        public string Message { get; set; } = string.Empty;

    }
}

