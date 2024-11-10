namespace Front_MVC.Models
{
    public class PostDto
    {
        public int Id { get; set; }
        public CommentDto? PinComment { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserDto Author { get; set; }
        public bool isEdited { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreateAt { get; set; }
        public int LikeCount { get; set; }
        public ICollection<CommentDto>? Replies { get; set; }
    }
}
