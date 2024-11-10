using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ForumModel.Context;
using ForumModel.DTO;
using ForumModel.Entities;
using ForumModel.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ForumModel.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<PostDto>> GetPageAsync<TKey>
            (Expression<Func<Post, TKey>> expression, int skipIndex = 0, int pageSize = 25)
        {
            var posts =  await _context.Posts
                .Where(x => !x.isDeleted)
                .Include(x => x.Author)
                .OrderByDescending(expression)
                .Skip(skipIndex)
                .Take(pageSize)
                .ToListAsync();

            var result = new List<PostDto>();
            foreach (var p in posts)
            {
                result.Add(new PostDto() { 
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    isEdited = p.isEdited,
                    isDeleted = p.isDeleted,
                    LikeCount = p.LikeCount,
                    CreateAt = p.CreateAt,
                    Author = new UserDto() { Id = p.Author.UserId, Nickname = p.Author.Nickname}
                });


            }

            return result;
        }
        public int GetPostCount()
        {
            return _context.Posts.Count();
        }

        /*
        private Comment GetAllReplies(int commentId)
        {

            var cm = _context.Comments
                .Include(x => x.Replies)
                .SingleOrDefault(x => x.Id == commentId);

            if (cm == null)
            {
                throw new NullReferenceException("El comentario no existe.");
            }

            if (cm.Replies != null)
            {
                for (int i = 0; i < cm.Replies.Count; i++)
                {
                    cm.Replies[i] = GetAllReplies(cm.Replies[i].Id);
                }
            }

            return cm;
        }*/
        public override async Task<Post> GetByIdAsync(int id)
        {
            return await _context.Set<Post>().Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
        }

        /*
        public Post GetPostAndReplies(int postId)
        {
            var post = _context.Posts
                .Include(x => x.Replies)
                .SingleOrDefault(x => x.Id == postId);

            if(post == null)
            {
                throw new NullReferenceException("El post no existe.");
            }

            if(post.Replies == null)
            {
                return post;
            }

            var replies = new List<Comment>();

            foreach (var reply in post.Replies)
            {
                replies.Add( GetAllReplies(reply.Id) );
            }

            post.Replies = replies;

            return post;
        }*/

        private CommentDto GetAllReplies(int commentId)
        {

            var cm = _context.Comments
                .Include(x => x.Author)
                .Include(x => x.Replies)
                .SingleOrDefault(x => x.Id == commentId);

            if (cm == null)
            {
                throw new NullReferenceException("El comentario no existe.");
            }

            var commentDto = new CommentDto()
            {
                Id = cm.Id,
                Description = cm.Description,
                CreateAt = cm.CreateAt,
                LikeCount = cm.LikeCount,
                isDeleted = cm.isDeleted,
                isEdited = cm.isEdited,
                Author = new UserDto() { Id = cm.Author.UserId, Nickname = cm.Author.Nickname}
            };

            if (cm.Replies != null)
            {
                var replies = new List<CommentDto>();

                foreach (var reply in cm.Replies)
                {
                    replies.Add( GetAllReplies(reply.Id) );
                }

                commentDto.Replies = replies;
            }

            return commentDto;
        }
        public PostDto GetPostAndReplies(int postId)
        {
            var post = _context.Posts
                .Include(x => x.PinComment)
                .ThenInclude(x => x.Author)
                .Include(x => x.Author)
                .Include(x => x.Replies)
                .SingleOrDefault(x => x.Id == postId);

            if (post == null)
            {
                throw new NullReferenceException("El post no existe.");
            }

            PostDto dto = new PostDto()
            {
                Title = post.Title,
                Description = post.Description,
                isDeleted = post.isDeleted,
                isEdited = post.isEdited,
                Id = post.Id,
                CreateAt = post.CreateAt,
                LikeCount = post.LikeCount,
                Author = new UserDto() { Id = post.Author.UserId, Nickname =  post.Author.Nickname },
            };

            if(post.PinComment != null)
            {
                var pinComment = new CommentDto()
                {
                    Description = post.PinComment.Description,
                    isDeleted = post.PinComment.isDeleted,
                    isEdited = post.PinComment.isEdited,
                    Id = post.PinComment.Id,
                    CreateAt = post.PinComment.CreateAt,
                    LikeCount = post.PinComment.LikeCount,
                    Author = new UserDto() 
                    { 
                        Id = post.PinComment.Author.UserId, Nickname = post.PinComment.Author.Nickname 
                    }
                };

                dto.PinComment = pinComment;
            }

            if (post.Replies == null)
            {
                return dto;
            }

            var replies = new List<CommentDto>();

            foreach (var reply in post.Replies)
            {
                replies.Add(GetAllReplies(reply.Id));
            }

            dto.Replies = replies;

            return dto;
        }

        public Comment? FindReply (int commentId, int replyToFindId)
        {
            var cm = _context.Comments.Include(x => x.Replies).FirstOrDefault(x => x.Id == commentId);

            if(cm == null || cm.Replies == null)
            {
                return null;
            }

            if (cm.Id == replyToFindId)
            {
                return cm;
            }

            foreach(var reply in cm.Replies)
            {
                var found = FindReply(reply.Id, replyToFindId);

                if (found != null)
                    return found;
            }

            return null;
        }

    }
}
