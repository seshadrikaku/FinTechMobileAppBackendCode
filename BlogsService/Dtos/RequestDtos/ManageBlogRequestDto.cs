namespace BlogsService.Dtos
{
    public class ManageBlogRequestDto
    {
        public int? Id { get; set; } // Nullable for Create, Required for Update
        public string Title { get; set; }
        public string Content { get; set; }

    }
}