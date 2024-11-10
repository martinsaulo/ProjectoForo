using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ForumModel.Entities
{
    public class Post : Comment
    {
        public string Title { get; set; }
        public Comment? PinComment { get; set; }

    }
}
