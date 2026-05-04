namespace BlogsService.Dtos
{
public class BlogListResponseDto
{
    public List<BlogResponseDto> Blogs { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class BlogResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public DateTime CreatedAt { get; set; }
    public AuthorDto Author { get; set; }   
}

    public class AuthorDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}