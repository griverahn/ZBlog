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

        [HttpPost("/api/register-admin")]
        public async Task<IActionResult> RegisterAdmin(UserDto userInfo)
        {
            return Ok(await _authentication.RegisterAdmin(userInfo));
        }
    }
}
