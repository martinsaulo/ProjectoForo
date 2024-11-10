using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumModel.DTO
{
    public class CommentDto
    {
        public int Id { get; set; }
        public UserDto Author { get; set; }
        public string Description { get; set; }
        public bool isEdited { get; set; }
        public bool isDeleted { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreateAt { get; set; }
        public ICollection<CommentDto>? Replies { get; set; }
    }
}
