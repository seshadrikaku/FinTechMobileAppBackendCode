using BlogsService.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogsService.Data
{
    public class BlogsDbContext  : DbContext
    {
        public BlogsDbContext(DbContextOptions<BlogsDbContext> options) : base(options) { }

        public DbSet<Blog> Blogs { get; set; }

  

    }
}
