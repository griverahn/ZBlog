using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize(Roles = "Public,Writer,Editor")]
        [HttpGet("/api/get-all-posts")]
        public async Task<IActionResult> GetAllPosts()
        {
            return Ok(await _postsAppService.GetAllPosts());
        }

        [Authorize(Roles = "Writer")]
        [HttpGet("/api/get-posts-by-user")]
        public async Task<IActionResult> GetPostsByUser(string userId)
        {
            return Ok(await _postsAppService.GetPostsByUser(userId));
        }

        [Authorize(Roles ="Editor")]
        [HttpGet("/api/get-pending-approval-posts")]
        public async Task<IActionResult> GetPendingApprovalPosts()
        {
            return Ok(await _postsAppService.GetPendingApprovalPosts());
        }

        [Authorize(Roles = "Public,Writer,Editor")]
        [HttpPost("/api/add-comment")]
        public async Task<IActionResult> AddComment(CommentDto comment)
        { 
            return Ok(await _postsAppService.AddComment(comment));
        }

        [Authorize(Roles = "Editor")]
        [HttpPost("/api/update-posts-status")]
        public async Task<IActionResult> UpdatePostsStatus(List<PostDto> editedPosts)
        {
            return Ok(await _postsAppService.UpdatePostsStatus(editedPosts));
        }

        [Authorize(Roles = "Writer")]
        [HttpPost("/api/add-post")]
        public async Task<IActionResult> AddPost(PostDto postDto)
        {
            return Ok(await _postsAppService.AddPost(postDto));
        }

        [Authorize(Roles = "Writer")]
        [HttpPatch("/api/edit-post")]
        public async Task<IActionResult> EditPost(PostDto postDto)
        {
            return Ok(await _postsAppService.EditPost(postDto));
        }

        [Authorize(Roles = "Writer")]
        [HttpPatch("/api/submit-post")]
        public async Task<IActionResult> SubmitPost(PostDto postDto)
        {
            return Ok(await _postsAppService.SubmitPost(postDto));
        }

    }
}
