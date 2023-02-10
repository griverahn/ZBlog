using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZBlogAPI.AppServices;
using ZBlogAPI.Models.DTO;

namespace ZBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        PostsAppService _postsAppService;
        public PostsController(PostsAppService postsAppService)
        {
            _postsAppService = postsAppService;
        }

        //[Authorize(Roles = "Admin,Public,Writer,Editor")]
        [HttpGet("/api/get-all-posts")]
        public async Task<IActionResult> GetAllPosts()
        {
            return Ok(await _postsAppService.GetAllPosts());
        }

        [HttpGet("/api/get-posts-by-user")]
        public async Task<IActionResult> GetPostsByUser(string userId)
        {
            return Ok(await _postsAppService.GetPostsByUser(userId));
        }

        //[Authorize(Roles = "Admin,Public,Writer,Editor")]
        [HttpPost("/api/add-comment")]
        public async Task<IActionResult> AddComment(CommentDto comment)
        { 
            return Ok(await _postsAppService.AddComment(comment));
        }
    }
}
