using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZBlogAPI.AppServices;
using ZBlogAPI.Models.DTO;

namespace ZBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        AuthenticateAppService _authentication;
        public AuthenticationController(AuthenticateAppService authentication)
        { 
            _authentication = authentication;
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> Login(LoginDto authentication)
        {
            return Ok(await _authentication.Login(authentication));            
        }

        [HttpPost("/api/add-user")]
        public async Task<IActionResult> AddUser(UserDto userInfo)
        {
            return Ok(await _authentication.AddUser(userInfo));
        }

        [HttpPost("/api/get-user-id")]
        public async Task<IActionResult> GetUserId(string userName)
        {
            return Ok(await _authentication.GetUserId(userName));
        }
    }
}
