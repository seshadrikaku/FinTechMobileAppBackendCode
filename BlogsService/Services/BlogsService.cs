
using System.Security.Claims;
using BlogsService.Data;
using BlogsService.Dtos;
using BlogsService.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Common;

namespace BlogsService.Services
{
    public class BlogsService : IBlogsService
    {

        private readonly BlogsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BlogsService(BlogsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        //Method to Get GUID of the currently logged in user from the JWT token
        private Guid GetUserIdFromToken()
        {
            var claim = _httpContextAccessor.HttpContext?.User
              .FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(claim, out var userId) ? userId : Guid.Empty;

        }


       public async Task<ApiResponse<CreateBlogRequestDto>> CreateBlog(CreateBlogRequestDto request)
        {
            var userId= GetUserIdFromToken();
            if (userId == Guid.Empty)
                return ApiResponse<CreateBlogRequestDto>.Fail("Unauthorized.", 401);
             if(request.Title == null || request.Content == null)
                return ApiResponse<CreateBlogRequestDto>.Fail("Title and Content are required.", 400);

            var blog = new Blog
            {
                Title = request.Title,
                Content = request.Content,
                ImageUrl = request.ImageUrl,
                AuthorName = "John Doe", // In real scenario, we can fetch this from user service using userId
                AuthorDescription = "A passionate blogger.", // This can also come from user service
                CreatedAt = DateTime.Now,
                CreatedBy = userId.ToString()
            };
            return ApiResponse<CreateBlogRequestDto>.Ok(new CreateBlogRequestDto
            {
                Title = blog.Title,
                Content = blog.Content,
                ImageUrl = blog.ImageUrl
            });
           
        }


        public async Task<ApiResponse<BlogListResponseDto>> GetAllBlogs(GetBlogRequestDto getBlogRequestDto)
        {

            var userId = GetUserIdFromToken();
            if (userId == Guid.Empty)
                return ApiResponse<BlogListResponseDto>.Fail("Unauthorized.", 401);

            var pageNumber = getBlogRequestDto.PageNumber <= 0 ? 1 : getBlogRequestDto.PageNumber;
            var pageSize = getBlogRequestDto.PageSize <= 0 ? 10 : getBlogRequestDto.PageSize;
            var query = _context.Blogs.AsNoTracking();

            var totalBlogs = await _context.Blogs.CountAsync();
            var blogs = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
                    .Select(x => new BlogResponseDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Content = x.Content,
                        ImageUrl = x.ImageUrl,
                        CreatedAt = x.CreatedAt,//Later we can format this date as per requirement
                        Author = new AuthorDto
                        {
                            Name = x.AuthorName,
                            Description = x.AuthorDescription
                        },
                        LikesCount = x.LikesCount,
                        CommentsCount = x.CommentsCount,
                        IsLikedByCurrentUser = x.IsLikedByCurrentUser,


                    })
        .ToListAsync();


            return ApiResponse<BlogListResponseDto>.Ok(new BlogListResponseDto
            {
                Blogs = blogs,
                TotalCount = totalBlogs,
                PageNumber = pageNumber,
                PageSize = pageSize
            });

        }

        //Method Manage the Blog (Create/Update)
        public async Task<ApiResponse<ManageBlogResponseDto>> ManageBlog(ManageBlogRequestDto request)
        {
            var userId = GetUserIdFromToken();
            if (userId == Guid.Empty)
                return ApiResponse<ManageBlogResponseDto>.Fail("Unauthorized.", 401);

            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == request.Id);


            if (blog == null)
                return ApiResponse<ManageBlogResponseDto>.Fail("Blog Id does not exist for update.", 400);

            blog.Title = request.Title;
            blog.Content = request.Content;
            blog.UpdatedAt = DateTime.Now;
            blog.CreatedBy = userId.ToString(); //We need to create modified By column also


            await _context.SaveChangesAsync();



            return ApiResponse<ManageBlogResponseDto>.Ok(new ManageBlogResponseDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content
            });

        }


        public async Task<ApiResponse<bool>> DeleteBlogById()
        {



            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> DeleteCommentBlogById()
        {
            throw new NotImplementedException();
        }



        public Task<ApiResponse<bool>> ManageCommentBlogById()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> ManageLikeBlogById()
        {
            throw new NotImplementedException();
        }
    }
}
