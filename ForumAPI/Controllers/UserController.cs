using ForumModel.Context;
using ForumModel.Entities;
using ForumModel.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost()]
        public IActionResult RegisterNewUser(string nickname, string email, string password)
        {
            if (password.Length < 8 || password.Length > 32 || !password.Any(char.IsDigit))
            {
                return ValidationProblem("Formato de contraseña invalido.");
            }

            if (_userRepository.CheckNickname(nickname))
            {
                return ValidationProblem("Nombre de usuario ya en uso.");
            }

            if (_userRepository.CheckEmail(email))
            {
                return ValidationProblem("Email ya en uso.");
            }

            try
            {
                ForumUser user = new ForumUser(nickname, email, password);

                _userRepository.Create(user);
                _userRepository.Save();
                return Ok();

            } catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        [HttpDelete()]
        public async Task<IActionResult> DeleteUser(int userId)
        {

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("El usuario no existe.");
            }

            try
            {
                _userRepository.Delete(user);
                await _userRepository.SaveAsync();

                return Ok();

            } catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet]
        public IActionResult isValidLogIn(string email, string password)
        {

            int userId = _userRepository.CheckLogIn(email, password);

            if (userId != -1)
            {
                return Ok(userId);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
