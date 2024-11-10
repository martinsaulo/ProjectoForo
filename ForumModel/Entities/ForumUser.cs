using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForumModel.Context;

namespace ForumModel.Entities
{
    public class ForumUser
    {
        [Key]
        public int UserId { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ForumUser(string nickname, string email, string password)
        {
            Nickname = nickname;
            Email = email;
            Password = password;
        }
        
    }
}
