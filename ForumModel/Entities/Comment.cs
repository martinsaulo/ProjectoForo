using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ForumModel.Context;

namespace ForumModel.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public ForumUser Author { get; set; }
        public bool isEdited { get; set; }
        public bool isDeleted { get; set; }
        public int LikeCount { get; set; }
        public List<Like>? Likes { get; set; }
        public List<Comment>? Replies { get; set; }
        public DateTime CreateAt { get; set; }

    }
}
