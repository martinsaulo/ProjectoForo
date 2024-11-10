using ForumModel.Entities;

namespace ForumAPI.DTO
{
    public class PostDTO
    {
        public string Title { get; set; }
        public CommentDTO? PinComment { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public bool isEdited { get; set; }
        public int LikeCount { get; set; }
        public List<Comment>? Replies { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
