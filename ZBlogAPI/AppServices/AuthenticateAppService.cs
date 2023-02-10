using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZBlogAPI.Models;
using ZBlogAPI.Models.DTO;

namespace ZBlogAPI.AppServices
{
    public class AuthenticateAppService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        //private readonly IConfiguration _configuration;

        public AuthenticateAppService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)//, IConfiguration configuration)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            //_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<AuthenticationDto> Login(LoginDto model)
        {
            var user = await userManager.FindByNameAsync(model.User);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ASHDTHWNSD123445DSA243F123F34F"));

                var token = new JwtSecurityToken(
                    issuer: "URLDelProyectoUI",
                    audience: "URLDelProyectoUI",
                    expires: DateTime.Now.AddHours(24),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new AuthenticationDto
                {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Role = user.Role,
                        Name = user.Name,
                        Expiration = token.ValidTo
                };
            }
            return new AuthenticationDto { Message = "Usuario o Clave son incorrectos" };
        }


        public async Task<AuthenticationDto> RegisterAdmin(UserDto model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);

            if (userExists != null)
            {
                return new AuthenticationDto { Message = "User already exists!" };
            }

            User user = new User()
            {
                UserName = model.UserName,                
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = model.Name
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new AuthenticationDto { Message = "User creation failed! Please check user details and try again." };
            }
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.RoleExistsAsync("Admin"))
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }

            return new AuthenticationDto { Message = "User created successfully!" };
        }
    }
}
