using ForumModel.Context;
using ForumModel.Entities;
using ForumModel.Repositories;
using ForumModel.Repositories.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace ForumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;
        private IUserRepository _userRepository;
        public PostController(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }


        [HttpPut("Comment/{commentId}")]
        public async Task<IActionResult> UpdateComment(int userId, int commentId, string content)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);

            if (comment == null || comment.isDeleted)
            {
                return NotFound("El comentario no existe");
            }

            if(comment.Author.UserId != userId )
            {
                return BadRequest("No tienes permisos para editar este comentario.");
            }

            try
            {
                comment.Description = content;
                comment.isEdited = true;
                
                _commentRepository.Update(comment);
                _commentRepository.Save();

                return Ok();

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("PinComment/{commentId}")]
        public async Task<IActionResult> PinComment(int userId, int postId, int commentId)
        {

            try
            {

                var post = await _postRepository.GetByIdAsync(postId);

                if (post.Author.UserId != userId)
                {
                    return BadRequest("El userId no coincide con el autor del post.");
                }

                var pinComment = _postRepository.FindReply(postId, commentId);

                if (pinComment == null || pinComment.isDeleted)
                {
                    return BadRequest("El comentario no existe.");
                }

                post.PinComment = pinComment;
                _postRepository.Update(post);
                _postRepository.Save();

                return Ok();

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{postId}")]
        public IActionResult GetPostAndReplies(int postId)
        {

            try
            {
                var result = _postRepository.GetPostAndReplies(postId);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId, int userId)
        {

            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
            {
                return NotFound("El posteo no existe");
            }

            if (post.Author.UserId != userId)
            {
                return BadRequest("No tienes permisos para eliminar este posteo.");
            }

            try
            {
                post.Description = "(Eliminado).";
                post.isDeleted = true;


                _postRepository.Update(post);
                _postRepository.Save();

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Comment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId, int userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);

            if(comment == null)
            {
                return NotFound("El comentario no existe");
            }

            if(comment.Author.UserId != userId)
            {
                return BadRequest("No tienes permisos para eliminar este comentario.");
            }

            try
            {
                comment.Description = "(Eliminado).";
                comment.isDeleted = true;


                _commentRepository.Update(comment);
                _commentRepository.Save();

                return Ok();

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int userId, int postId, string content, string title)
        {

            try
            {

                var post = await _postRepository.GetByIdAsync(postId);

                if (post == null)
                {
                    return NotFound("El posteo no existe.");
                }

                if (post.Author.UserId != userId || post.isDeleted)
                {
                    return BadRequest("No tienes permisos para editar este posteo.");
                }

                post.Title = title;
                post.Description = content;
                post.isEdited = true;

                _postRepository.Update(post);
                await _postRepository.SaveAsync();

                return Ok();

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(string title, string content, int authorId)
        {
            var author = await _userRepository.GetByIdAsync(authorId);

            if(author == null)
            {
                return NotFound("El usuario no existe.");
            }

            Post post = new Post() 
            {
                Title = title,
                Description = content,
                CreateAt = DateTime.Now,
                LikeCount = 0,
                isEdited = false,
                isDeleted = false,
                Author = author
            };

            try
            {
                _postRepository.Create(post);
                _postRepository.Save();

                return Ok();

            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            } 
        }

        [HttpGet]
        public async Task<IActionResult> GetPage(int pageNro, int pageSize, string orderBy)
        {
            int postCount = _postRepository.GetPostCount();

            if ((postCount / pageSize) + 1 < pageNro)
            {
                return BadRequest("Número de página fuera de rango.");
            }

            int startIndex = (pageNro - 1) * pageSize;

            orderBy = orderBy.ToLower();

            switch (orderBy)
            {
                default:
                    return BadRequest("Tipo invalido para el orderBy");

                case "date":
                    {
                        var response = await _postRepository
                            .GetPageAsync(x => x.CreateAt, startIndex, pageSize);

                        response = response.Where(x => !x.isDeleted);

                        return Ok(response);
                    }
                    
                case "likecount":
                    {
                        var response = await _postRepository
                            .GetPageAsync(x => x.LikeCount, startIndex, pageSize);

                        response = response.Where(x => !x.isDeleted);

                        return Ok(response);
                    }
            }
        }

        [HttpPost("Reply/{commentId}")]
        public async Task<IActionResult> ReplyComment(int commentId, string content, int userId)
        {

            var user = await _userRepository.GetByIdAsync(userId);

            if(user == null)
            {
                return BadRequest("El usuario no existe.");
            }

            Comment reply = new Comment()
            {
                Description = content,
                CreateAt = DateTime.Now,
                isEdited = false,
                isDeleted = false,
                LikeCount = 0,
                Author = user
            };

            var comment = await _commentRepository.GetByIdAsync(commentId);

            if(comment == null || comment.isDeleted )
            {
                return BadRequest("El comentario no existe.");
            }


            try
            {
                if (comment.Replies == null) comment.Replies = new List<Comment>();

                comment.Replies.Add(reply);

                _commentRepository.Update(comment);
                await _commentRepository.SaveAsync();

                return Ok();

            }catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        
        [HttpPut("Like/{commentId}")]
        public async Task<IActionResult> LikeComment(int commentId, int userId, bool isDislike_)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("El usuario no existe.");
            }

            var comment = await _commentRepository.GetCommentAndLikesAsync(commentId);

            if(comment == null || comment.isDeleted)
            {
                return BadRequest("El comentario no existe.");
            }


            if (comment.Likes == null) comment.Likes = new List<Like>();

            int likeIndex = comment.Likes.FindIndex(x => x.User.UserId == userId);


            Like like;

            if (likeIndex != -1)
            {
                like = comment.Likes[likeIndex];

                if(like.isDislike != isDislike_) comment.LikeCount += (isDislike_) ? -2 : 2;

                like.isDislike = isDislike_;

                comment.Likes[likeIndex] = like;
            }
            else
            {
                like = new Like()
                {
                    isDislike = isDislike_,
                    Comment = comment,
                    User = user
                };

                comment.Likes.Add(like);
                comment.LikeCount += (like.isDislike) ? -1 : 1;
            }

            
            try
            {
                _commentRepository.Update(comment);
                _commentRepository.Save();
                return Ok();


            }catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
