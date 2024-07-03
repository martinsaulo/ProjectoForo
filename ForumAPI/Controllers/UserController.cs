using ForumModel;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost()]
        public ActionResult RegisterNewUser(string nickname, string email, string password)
        {
            try
            {
                ForumUser.isValidNickname(nickname);
                ForumUser.isValidEmail(email);
                ForumUser.isValidPassword(password);
                
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    ForumUser newUser = new ForumUser(nickname, email, password);
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete()]
        public ActionResult DeleteUser(int userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.Find(userId);

                if(user == null)
                {
                    return NotFound("El usuario no existe.");
                }
                else
                {
                    db.Users.Remove(user);
                    db.SaveChanges();

                    return Ok();
                }
            }
        }
        [HttpGet()]
        public ActionResult isValidLogIn(string email, string password)
        {
            ForumUser? user;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                user = db.Users.FirstOrDefault(x => x.Email == email);
            }

            if (user == null)
            {
                return NotFound("Email no registrado");
            }
            if (user.Password != password)
            {
                return BadRequest("Contraseña incorrecta.");
            }

            return Ok(user.UserId);
        }
    }
}
