using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZBlogAPI.DataContext;
using ZBlogAPI.Models;
using ZBlogAPI.Models.DTO;

namespace ZBlogAPI.AppServices
{
    public class AuthenticateAppService
    {
        private const string uniqueValidator = "ASHDTHWNSD123445DSA243F123F34F";
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        ApplicationDBContext _dbContext;

        public AuthenticateAppService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDBContext dbContext)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _dbContext = dbContext;
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

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(uniqueValidator));

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
            return new AuthenticationDto { Message = "User or Password are wrong!" };
        }


        public async Task<AuthenticationDto> AddUser(UserDto model)
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
                Name = model.Name,
                Role = model.Role
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new AuthenticationDto { Message = "User creation failed! Please check user details and try again." };
            }
            if (!await roleManager.RoleExistsAsync(user.Role))
            {
                await roleManager.CreateAsync(new IdentityRole(user.Role));
            }
            if (await roleManager.RoleExistsAsync(user.Role))
            {
                await userManager.AddToRoleAsync(user, user.Role);
            }

            return new AuthenticationDto { Message = "User created successfully!" };
        }

        public async Task<string> GetUserId(string userName)
        {
            return _dbContext.Users.FirstOrDefault(s=>s.UserName == userName).Id;
        }
    }
}
