using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ForumModel
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        public ForumUser User { get; set; }
        public Comment Comment { get; set; }
        public bool isDislike { get; set; }
        
    }
}
