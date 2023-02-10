using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using ZBlogAPI.DataContext;
using ZBlogAPI.Models;
using ZBlogAPI.Models.DTO;

namespace ZBlogAPI.AppServices
{
    public class PostsAppService
    {
        ApplicationDBContext _dbContext;
        const string New = "New";
        const string PendingApproval = "Pending Approval";
        const string Approved = "Approved";
        const string Rejected = "Rejected";

        public PostsAppService(ApplicationDBContext dbContext) { 
            _dbContext= dbContext;

        }
                                                                                                                                                                                                   
        public async Task<List<PostDto>> GetAllPosts()
        {
            var dbPosts = await _dbContext.Posts.Where(s=>s.Status == Approved).ToListAsync(); 

            return await ConvertPostsToDto(dbPosts);

        }

        public async Task<List<PostDto>> GetPendingApprovalPosts()
        {
            var dbPosts = await _dbContext.Posts.Where(s=>s.Status == PendingApproval).ToListAsync();

            return await ConvertPostsToDto(dbPosts);

        }

        public async Task<List<PostDto>> GetPostsByUser(string UserId)
        {
            var dbPosts = await _dbContext.Posts.Where(s => s.UserId == UserId).ToListAsync();
            var result = new List<PostDto>();

            return await ConvertPostsToDto(dbPosts);
        }

        public async Task<List<PostDto>> UpdatePostsStatus(List<PostDto> editedPosts)
        {            
            var dbPosts = await _dbContext.Posts.Where(s => editedPosts.Select(x=>x.Id).Contains(s.Id))
                                                .ToListAsync();

            foreach (var editedPost in editedPosts)
            {
                foreach (var dbPost in dbPosts)
                {
                    if (editedPost.Id == dbPost.Id)
                    {
                        if (editedPost.Status == Approved)
                        {
                            dbPost.Status = Approved;
                        }
                        else { 
                            //return invalid status attempt
                        }


                        if (editedPost.Status == Rejected)
                        {
                            dbPost.Status = Approved;
                        }
                        else {
                            //return invalid status attempt
                        }

                    }
                }
            }

            //Manejar excepciones aqui
            await _dbContext.SaveChangesAsync();

            return await ConvertPostsToDto(dbPosts);
        }

        public async Task<CommentDto> AddComment(CommentDto commentDto)
        {
            try
            {
                await _dbContext.Comments.AddAsync(new Comment()
                {
                    Description = commentDto.Description,
                    UserId = commentDto.UserId,
                    PostId = commentDto.PostId
                });
               
                await _dbContext.SaveChangesAsync();

                return new CommentDto { Message = "Comment created successfully!" };
            }
            catch (Exception)
            {
                return new CommentDto { Message = "Error while creating the comment..." };
            }            
        }

        public async Task<PostDto> AddPost(PostDto postDto)
        {
            try
            {
                await _dbContext.Posts.AddAsync(new Post()
                {
                    Title = postDto.Title,
                    Description = postDto.Description,
                    UserId = postDto.UserId,
                    PublishingDate = postDto.PublishingDate,
                    Status = New
                });

                await _dbContext.SaveChangesAsync();

                return new PostDto { Message = "Post created successfully!" };
            }
            catch (Exception)
            {
                return new PostDto { Message = "Error while creating the post..." };
            }
        }

        public async Task<PostDto> EditPost(PostDto postDto)
        {
            Post dbPost = await GetSpecificPost(postDto);

            if (dbPost != null)
            {
                if (dbPost.Status == New)
                {
                    try
                    {
                        dbPost.Title = postDto.Title;
                        dbPost.Description = postDto.Description;
                        dbPost.Status = postDto.Status;
                        postDto.Message = "Post successfully updated.";

                        await SaveChanges();
                        return postDto;
                    }
                    catch (Exception)
                    {
                        return new PostDto { Message = "Error while updating the post..." };
                    }

                }
                else
                {
                    return new PostDto { Message = "Unable to edit post, it is already " + dbPost.Status };
                }
            }
            else
            {
                return new PostDto { Message = "There are no posts to edit..." };
            }
        }

        public async Task<PostDto> SubmmitPost(PostDto postDto)
        {
            Post dbPost = await GetSpecificPost(postDto);

            if (dbPost != null)
            {
                if (dbPost.Status == New || dbPost.Status == Rejected)
                {
                    try
                    {
                        dbPost.Status = PendingApproval;
                        postDto.Message = "Post successfully updated.";

                        await SaveChanges();
                        return postDto;
                    }
                    catch (Exception)
                    {
                        return new PostDto { Message = "Error while updating the post..." };
                    }

                }
                else
                {
                    return new PostDto { Message = "Unable to edit post, it is already " + dbPost.Status };
                }
            }
            else
            {
                return new PostDto { Message = "There are no posts to edit..." };
            }
        }

        private async Task<Post> GetSpecificPost(PostDto postDto)
        {
            return await _dbContext.Posts.Where(s => s.Id == postDto.Id
                                         && s.UserId == postDto.UserId)
                                         .FirstOrDefaultAsync();
        }

        private async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        private async Task<string> GetUserName(string userId)
        {
            //TODO: Revisar mejores practicas
            return await _dbContext.Users.Where(s => s.Id == userId)
                                         .Select(x => x.UserName)
                                         .FirstOrDefaultAsync();
        }

        private async Task<List<PostDto>> ConvertPostsToDto(List<Post> dbPosts)
        {
            var result = new List<PostDto>();

            foreach (var post in dbPosts)
            {
                var postDto = new PostDto()
                {
                    Title = post.Title,
                    Description = post.Description,
                    UserName = await GetUserName(post.UserId),
                    PublishingDate = post.PublishingDate,
                    Status = post.Status
                };
                result.Add(postDto);
            }

            return result;
        }

    }
}
