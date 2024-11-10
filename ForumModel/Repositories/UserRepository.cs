using ForumModel.Context;
using ForumModel.Entities;
using ForumModel.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumModel.Repositories
{
    public class UserRepository : Repository<ForumUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context){ }

        public bool CheckNickname(string nickname)
        {
            return _context.Users.Any(u => u.Nickname == nickname);
        }

        public int CheckLogIn(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if(user == null || !(user.Password == password) )
            {
                return -1;
            }

            return user.UserId;
        }
        public bool CheckEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}
