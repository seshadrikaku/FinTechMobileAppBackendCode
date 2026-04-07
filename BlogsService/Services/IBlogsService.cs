namespace BlogsService.Services;
using global::BlogsService.Dtos;
using global::BlogsService.Models;
using Shared.Common;


public interface IBlogsService
    {

        public Task<ApiResponse<CreateBlogRequestDto>> CreateBlog(CreateBlogRequestDto request);
        public Task<ApiResponse<BlogListResponseDto>> GetAllBlogs(GetBlogRequestDto request);
        public Task<ApiResponse<ManageBlogResponseDto>> ManageBlog(ManageBlogRequestDto request);
        public Task<ApiResponse<bool>> DeleteBlogById();
        public Task<ApiResponse<bool>> ManageLikeBlogById();
        public Task<ApiResponse<bool>> ManageCommentBlogById();
        public Task<ApiResponse<bool>> DeleteCommentBlogById();
        
    }

