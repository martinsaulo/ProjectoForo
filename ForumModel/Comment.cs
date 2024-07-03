using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace ForumModel
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public ForumUser Author { get; set; }
        public bool isEdited { get; set; }
        public int LikeCount { get; set; }
        public List<Like>? Likes { get; set; }
        public List<Comment>? Replies { get; set; }
        public DateTime CreateAt { get; set; }
        public static Comment GetComment(int commentId)
        {
            Comment? cm;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                cm = db.Comments
                    .Include(x => x.Replies)
                    .SingleOrDefault(x => x.Id == commentId);
            }

            if (cm == null)
            {
                throw new NullReferenceException("El comentario no existe.");
            }

            if (cm.Replies != null)
            {
                for (int i = 0; i < cm.Replies.Count; i++)
                {
                    cm.Replies[i] = GetComment(cm.Replies[i].Id);
                }
            }

            return cm;
        }
        public void Edit(string newContent, int userId)
        {
            if(userId != Author.UserId)
            {
                throw new ArgumentException("UserId no coincide con el del autor.");
            }
            if(newContent == "")
            {
                throw new ArgumentException("El contenido no puede ser vacio.");
            }

            isEdited = true;
            Description = newContent;
        }
        public void LikeComment(bool isDislike_, ForumUser user)
        {
            if(Likes == null)
            {
                Likes =  new List<Like>();
            }

            Like like = new Like() 
            {
                User = user,
                Comment = this,
                isDislike = isDislike_,
            };
            LikeCount += (isDislike_)? -1 : 1 ;

            Likes.Add(like);
        }
        public void Reply(string content, ForumUser replyAuthor) 
        {
            if(content == "")
            {
                throw new ArgumentException("El contenido no puede ser vacio.");
            }

            Comment reply = new Comment() 
            {
                Description = content,
                Author = replyAuthor,
                CreateAt = DateTime.Now,
                LikeCount = 0,
                isEdited = false,
            };

            if(Replies == null)
            {
                Replies = new List<Comment>();
            }
            Replies.Add(reply);
        }
    }
}
