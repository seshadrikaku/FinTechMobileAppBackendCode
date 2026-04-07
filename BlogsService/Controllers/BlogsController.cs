using BlogsService.Dtos;
using BlogsService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogsService.Controllers
{
    [Authorize]
    [Route("api/blogs")]
    [ApiController]
    public class BlogsController : ControllerBase
    {

       private readonly IBlogsService _blogsService;

       

       public BlogsController(IBlogsService blogsService)
        {
            _blogsService = blogsService;
        }

       

        //Get All Blogs
        [HttpGet("get-all-blogs")]
        public async Task<IActionResult> GetAllBlogs(GetBlogRequestDto getBlogRequestDto)
        {
            var blogs = await _blogsService.GetAllBlogs(getBlogRequestDto);
            return Ok(blogs);
        }


        //Manage Blog
        [HttpPost("manage-blog")]
        public async Task<IActionResult> ManageBlog()
        {
            return Ok("Blog Managed");
        }

        //Delete Blog By Id
        [HttpDelete("delete-blog-by-id")]
        public async Task<IActionResult> DeleteBlogById()
        {
            return Ok("Blog Deleted");
        }

        //Like Blog By Id
        [HttpPost("manage-like-blog-by-id")]
        public async Task<IActionResult> ManageLikeBlogById()
        {
            return Ok("Blog Liked");
        }

        //Manage Comment Blog By Id
        [HttpPost("manage-comment-blog-by-id")]
        public async Task<IActionResult> ManageCommentBlogById()
        {
            return Ok("Blog Commented");
        }

        //Delete Comment Blog By Id
        [HttpDelete("delete-comment-blog-by-id")]
        public async Task<IActionResult> DeleteCommentBlogById()
        {
            return Ok("Blog Comment Deleted");
        }
        
    }
}
