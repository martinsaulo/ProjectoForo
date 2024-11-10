namespace Front_MVC.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public UserDto Author { get; set; }
        public DateTime CreateAt { get; set; }
        public bool isEdited { get; set; }
        public bool isDeleted { get; set; }
        public int LikeCount { get; set; }
        public ICollection<CommentDto>? Replies { get; set;}
    }
}
