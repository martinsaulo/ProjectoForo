using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ForumModel.Entities;
using ForumModel.DTO;


namespace ForumModel.Repositories.Contracts
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<PostDto>> GetPageAsync<TKey>(Expression<Func<Post, TKey>> expression, int skipIndex = 0, int pageSize = 25);
        PostDto GetPostAndReplies(int postId);
        Comment? FindReply(int commentId, int replyToFindId);
        int GetPostCount();
    }
}
