using ForumModel.Entities;

namespace ForumAPI.DTO
{
    public class CommentDTO
    {
        public string Description { get; set; }
        public string Author { get; set; }
        public bool isEdited { get; set; }
        public bool isDeleted { get; set; }
        public int LikeCount { get; set; }
        public List<CommentDTO>? Replies { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
