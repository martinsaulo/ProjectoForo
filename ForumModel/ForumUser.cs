using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumModel
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
        public static bool isValidNickname(string nickname)
        {
            bool alreadyExist;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                alreadyExist = db.Users.Any(u => u.Nickname == nickname);
            }

            if (alreadyExist)
            {
                throw new ArgumentException("Nickname ya en uso.");
            }

            return true;
        }
        public static bool isValidEmail(string email)
        {
            if (!email.EndsWith(".com") || !email.Contains("@"))
            {
                throw new ArgumentException("Formato de email invalido.");
            }

            bool alreadyExist;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                alreadyExist = db.Users.Any(u => u.Email == email);
            }

            if (alreadyExist)
            {
                throw new ArgumentException("Email ya registrado.");
            }

            return true;
        }
        public static bool isValidPassword(string password)
        {
            if (password.Length < 8 || password.Length > 32)
            {
                throw new ArgumentException("La cotraseña debe contener entre 8 y 32 caracteres.");
            }

            if (!password.Any(char.IsDigit))
            {
                throw new ArgumentException("La contraseña debe contener al menos un número.");
            }

            return true;
        }
    }
}
