using ForumModel.Context;
using ForumModel.Entities;
using ForumModel.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumModel.Repositories
{
    public class CommentRepository : Repository<Comment> , ICommentRepository 
    {
        public CommentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Comment> GetCommentAndLikesAsync(int id)
        {
            return await _context.Comments
                .Include(x => x.Likes)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.Set<Comment>().Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
