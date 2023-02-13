using Microsoft.EntityFrameworkCore;
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
        private const string Writer = "Writer";

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
            if (editedPosts.Any())
            {
                var dbPosts = await _dbContext.Posts.Where(s => editedPosts.Select(x => x.Id).Contains(s.Id))
                                                    .ToListAsync();
                var hasError = false;

                foreach (var dbPost in dbPosts)
                {
                    PostDto editedPost = editedPosts.FirstOrDefault(s => s.Id == dbPost.Id);

                    if (editedPost != null)
                    {
                        if (IsValidStatus(editedPost.Status))
                        {
                            dbPost.Status = editedPost.Status;
                            dbPost.ApprovalComments = editedPost.ApprovalComments;
                        }
                        else
                        {
                            hasError = true;                            
                            editedPosts.FirstOrDefault(s => s.Id == editedPost.Id).Message = "Incorrect Status.";
                        }
                    }
                }

                if (!hasError)
                {
                    await _dbContext.SaveChangesAsync();
                }
            }
            return editedPosts;
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

        public async Task<PostDto> SubmitPost(PostDto postDto)
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

        private async Task<List<PostDto>> ConvertPostsToDto(List<Post> dbPosts)
        {
            var result = new List<PostDto>();
            var usersIds = dbPosts.Select(s => s.UserId);
            var users = await _dbContext.Users.Where(s => usersIds.Contains(s.Id)).ToListAsync();

            foreach (var post in dbPosts)
            {
                var postDto = new PostDto()
                {
                    Title = post.Title,
                    Description = post.Description,
                    UserName = users.FirstOrDefault(s => s.Id == post.UserId).Name,
                    PublishingDate = post.PublishingDate,
                    Status = post.Status,
                    ApprovalComments = users.FirstOrDefault(s => s.Id == post.UserId).Role == Writer ? post.ApprovalComments : string.Empty
                };
                result.Add(postDto);
            }

            return result;
        }

        private bool IsValidStatus(string status)
        {
            List<string> validStates = new List<string>
            {
                Approved,
                Rejected
            };

            return validStates.Contains(status);
        }

    }
}
