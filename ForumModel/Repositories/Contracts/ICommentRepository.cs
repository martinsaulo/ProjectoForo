using ForumModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumModel.Repositories.Contracts
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment> GetCommentAndLikesAsync(int id);
    }
}
