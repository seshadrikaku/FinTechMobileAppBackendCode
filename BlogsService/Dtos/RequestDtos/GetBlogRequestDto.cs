namespace BlogsService.Dtos
{
    public class GetBlogRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
  }
}
