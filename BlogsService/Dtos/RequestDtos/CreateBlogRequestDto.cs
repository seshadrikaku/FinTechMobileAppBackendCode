namespace BlogsService.Dtos
{
    public class CreateBlogRequestDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
    }
}