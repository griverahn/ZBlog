using System.ComponentModel.DataAnnotations;

namespace ZBlogAPI.Models.DTO
{
    public class UserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
