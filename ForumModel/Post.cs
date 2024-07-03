using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ForumModel
{
    public class Post : Comment
    {
        public string Title { get; set; }
        public Comment? PinComment { get; set; }

        public void AddNewPinComment(int commentId)
        {
            if(Replies == null)
            {
                Replies = new List<Comment>();
            }

            Comment? comment = Replies.FirstOrDefault(x => x.Id == commentId);
            if (comment == null) 
            {
                throw new ArgumentException("Comentario no encontrado.");
            }

            PinComment = comment;
        }
        public void EditTitle(string newTitle, int userId)
        {
            if (userId != Author.UserId)
            {
                throw new ArgumentException("userId no coincide con el del autor.");
            }
            if (newTitle == "")
            {
                throw new ArgumentException("El contenido no puede ser vacio.");
            }

            Title = newTitle;
            isEdited = true;
        }
        public static Post CreatePost(string postContent, string title, ForumUser author)
        {
            Post newPost = new Post() 
            {
                Title = title,
                Description = postContent,
                Author = author,
                CreateAt = DateTime.Now,
                LikeCount = 0,
                isEdited = false,
            };

            return newPost;
        }
       
    }
}
