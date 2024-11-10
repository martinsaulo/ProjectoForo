using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForumModel.Repositories.Contracts;
using ForumModel.Entities;

namespace ForumModel.Repositories.Contracts
{
    public interface IUserRepository : IRepository<ForumUser>
    {
        bool CheckNickname(string nickname);
        int CheckLogIn(string email, string password);
        bool CheckEmail(string email);
    }
}
