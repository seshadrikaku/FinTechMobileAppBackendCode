namespace BlogsService.Models
{

    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string AuthorName { get; set; }

        public string AuthorDescription { get; set; }


        public string CreatedBy { get; set; }
        public string ImageUrl { get; set; }
        public int LikesCount { get; set; }

        public bool IsLikedByCurrentUser { get; set; }

        public int CommentsCount { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

    }
}
