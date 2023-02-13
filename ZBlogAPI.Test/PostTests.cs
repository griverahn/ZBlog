using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZBlogAPI.AppServices;
using ZBlogAPI.Controllers;
using ZBlogAPI.DataContext;
using ZBlogAPI.Models;
using ZBlogAPI.Models.DTO;

namespace ZBlogAPI.Test
{
    public class PostTests
    {
        [Fact]
        public async Task Test1()
        {
            var random = new Random(9999999);
            string databaseName = Guid.NewGuid().ToString() + random.Next(0, 9999999).ToString();

            DbContextOptions<ApplicationDBContext> options = new DbContextOptionsBuilder<ApplicationDBContext>().UseInMemoryDatabase(databaseName).Options;
            ApplicationDBContext dbContext = new ApplicationDBContext(options);
            PostsAppService postsAppService = new PostsAppService(dbContext);
            PostsController postController = new PostsController(postsAppService);

            User user = new User() { 
                Id = Guid.NewGuid().ToString()
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            Post post = new Post()
            {
                Description= "Test",
                Status="Approved",
                Title= "Test",
                UserId= user.Id,
                PublishingDate= DateTime.Now,
            };

            dbContext.Posts.Add(post);
            dbContext.SaveChanges();

            IActionResult actionResult = await postController.GetAllPosts();

            OkObjectResult result = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            IEnumerable<PostDto> posts = result.Value.As<IEnumerable<PostDto>>();

            posts.Should().HaveCount(1);

        }
    }
}