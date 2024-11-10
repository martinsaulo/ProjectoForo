using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;

namespace Front_MVC.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }

        public async Task<ActionResult> CreateAccount(string nickname, string email, string password)
        {
            HttpClient sharedClient = new()
            {
                BaseAddress = new Uri("http://localhost:5127"),
            };

            string 
                nicknameEncoded = HttpUtility.UrlEncode(nickname),
                emailEncoded = HttpUtility.UrlEncode(email),
                passwordEncoded = HttpUtility.UrlEncode(password);

            var response = await
                sharedClient.PostAsync($"api/User?nickname={nicknameEncoded}&email={emailEncoded}&password={passwordEncoded}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cuenta registrada exitosamente. Inicie sesión para continuar.";

                return RedirectToAction("SignIn", "Auth");
            }
            else
            {
                TempData["ErrorMessage"] = "Error al registrarse. Verifica tus datos e inténtalo de nuevo.";

                return RedirectToAction("SignUp", "Auth");
            }
        }


        public async Task<IActionResult> LogIn(string email, string password)
        {
            HttpClient sharedClient = new()
            {
                BaseAddress = new Uri("http://localhost:5127"),
            };


            string emailEncoded = HttpUtility.UrlEncode(email),
                passwordEncoded = HttpUtility.UrlEncode(password);

            var response = await sharedClient.GetAsync($"api/User?email={emailEncoded}&password={passwordEncoded}");


            if (!response.IsSuccessStatusCode)
            {

                TempData["ErrorMessage"] = "Error al iniciar sesión. Verifica tus credenciales e inténtalo de nuevo.";

                return RedirectToAction("SignIn", "Auth");
            }

            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            int userId = JsonConvert.DeserializeObject<int>(responseBody);

            HttpContext.Session.SetInt32("SESSION", userId);

            TempData["SuccessMessage"] = "Inicio de sesión exitoso.";

            return RedirectToAction("Index", "Home");
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("SESSION");

            return RedirectToAction("SignIn", "Auth");
        }
    }
}
