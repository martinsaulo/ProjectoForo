using ForumModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        const int PAGE_SIZE = 50;

        [HttpPut("/posts/{postId}")]
        public ActionResult EditPost(int userId ,int postId, string newContent, string newTitle)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var post = db.Posts
                    .Include(x => x.Author)
                    .SingleOrDefault(x => x.Id == postId);

                if (post == null)
                {
                    return NotFound("El posteo no existe");
                }

                try
                {
                    post.Edit(newContent, userId);
                    post.EditTitle(newTitle, userId);

                    db.SaveChanges();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("{commentId}")]
        public ActionResult EditComment(int userId, int commentId, string newContent)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var comment = db.Comments
                    .Include(x => x.Author)
                    .SingleOrDefault(x => x.Id == commentId);

                if(comment == null)
                {
                    return NotFound("El comentario no existe.");
                }

                try
                {
                    comment.Edit(newContent, userId);

                    db.SaveChanges();
                    return Ok();

                }catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost()]
        public ActionResult CreatePost(int userId, string title, string content)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.Find(userId);
                if (user == null)
                {
                    return NotFound("El usuario no existe.");
                }

                Post newPost = Post.CreatePost(content, title, user);

                db.Posts.Add(newPost);
                db.SaveChanges();

                return Ok();
            }
        }

        [HttpPut("{postId}/pin/{commentId}")]
        public ActionResult PinComment(int userId, int postId, int commentId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var post = db.Posts
                    .Include(x => x.Author)
                    .Include(x => x.Replies)
                    .SingleOrDefault(x => x.Id == postId);

                if(post == null)
                {
                    return NotFound("El post no existe.");
                }

                if(post.Author.UserId != userId)
                {
                    return BadRequest("El userId no coincide.");
                }

                try
                {
                    post.AddNewPinComment(commentId);
                    db.SaveChanges();

                    return Ok();

                }catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("{commentId}/replies/")]
        public ActionResult ReplyComment(int userId, int commentId, string content)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var comment = db.Comments.Find(commentId); 

                if (comment == null) 
                {
                    return NotFound("El comentario no existe.");
                }
                
                var user = db.Users.Find(userId);

                if(user == null)
                {
                    return NotFound("El usuario no existe.");
                }

                comment.Reply(content, user);
                db.SaveChanges(); 
                return Ok();
            }
        }

        [HttpGet("{postId}")]
        public ActionResult GetPost(int postId)
        {
            try
            {
                var post = Comment.GetComment(postId);

                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        public ActionResult GetPosts(string orderBy, int pageNro,  bool ascendingOrder)
        {
            // orderBy = (likeCount, Date)
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                int postCount = db.Posts.AsNoTracking().Count();
                if ( (postCount / PAGE_SIZE) + 1 < pageNro)
                {
                    return BadRequest("Número de página fuera de rango.");
                }
                
                int startIndex = (pageNro - 1) * PAGE_SIZE;

                orderBy = orderBy.ToLower();

                switch(orderBy)
                {
                    default:
                        return BadRequest("Tipo invalido para el orderBy");

                    case "date":
                        return Ok(
                            db.Posts.AsNoTracking()
                            .Skip(startIndex)
                            .OrderBy(x => x.CreateAt)
                            .Take(PAGE_SIZE)
                            .ToList()
                            );

                    case "likecount":
                        return Ok(
                            db.Posts.AsNoTracking()
                            .Skip(startIndex)
                            .OrderBy(x => x.LikeCount)
                            .Take(PAGE_SIZE)
                            .ToList()
                            );
                }
            }
        }

        [HttpPost("{commentId}/likes")]
        public ActionResult LikeComment(int commentId, int userId, bool isDislike) 
        {
            using (ApplicationDbContext db = new ApplicationDbContext()) 
            {
                ForumUser? user = db.Users.Find(userId);

                if (user == null)
                {
                    return NotFound("Usuario no encontrado.");
                }

                Comment? comment = db.Comments.Find(commentId);

                if(comment == null)
                {
                    return NotFound("Comentario no encontrado");
                }

                comment.LikeComment(isDislike, user);

                db.SaveChanges();
                return Ok();
            }
        }

        [HttpDelete("{postId}")]
        public ActionResult DeleteComment(int postId, int userId) 
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Comment? postToDelete = db.Comments
                    .Include(x => x.Author)
                    .SingleOrDefault(x => x.Id == postId);

                if (postToDelete == null)
                {
                    return NotFound("Posteo no encontrado.");
                }

                if (postToDelete.Author.UserId != userId)
                {
                    return BadRequest("El userId no coincide con el del autor del posteo.");
                }

                db.Remove(postToDelete);
                db.SaveChanges();
                return Ok();
            }
        }
    }
}
