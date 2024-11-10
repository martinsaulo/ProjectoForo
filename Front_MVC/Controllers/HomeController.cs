using Front_MVC.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web;
using Newtonsoft.Json;

namespace Front_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:5127"),
        };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 5, string orderBy = "date")
        {
            var response = await _httpClient
                .GetAsync($"api/Post?pageNro={page}&pageSize={pageSize}&orderBy={orderBy}");

            if(response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var posts = JsonConvert.DeserializeObject<List<PostDto>>(responseBody);


                // Devolver lista vacia en caso de pagina fuera de rango
                if (page == 10)
                {
                    var emptyList = new List<PostDto>();
                    return View(emptyList);
                }

                return View(posts);
            }
            else
            {
                var emptyList = new List<PostDto>();
                return View(emptyList);
            }

            
        }

        public async Task<IActionResult> PostDetails(int postId)
        {
            var response = await _httpClient
                .GetAsync($"api/Post/{postId}");

            if(response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var post = JsonConvert.DeserializeObject<PostDto>(responseBody);

                return View(post);
            }
            else
            {
                TempData["ErrorMessage"] = "Error, el posteo no existe.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> CreatePost(string title, string content, string authorId)
        {
            string titleEncoded = HttpUtility.UrlEncode(title),
                contentEncoded = HttpUtility.UrlEncode(content);

            var response = await _httpClient.PostAsync($"api/Post?title={titleEncoded}&content={contentEncoded}&authorId={authorId}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = $"Se ha publicado: {title}";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Error al publicar, intentelo más tarde.";
                return RedirectToAction("NewPost", "Home");
            }

        }

        public async Task<IActionResult> EditPost(string title, string content, string authorId, string postId)
        {
            string titleEncoded = HttpUtility.UrlEncode(title),
                contentEncoded = HttpUtility.UrlEncode(content);

            var response = await _httpClient.PutAsync
                ($"api/Post/{postId}?title={titleEncoded}&content={contentEncoded}&userId={authorId}", null);

            if(!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al publicar, intentelo más tarde.";
            }

            return RedirectToAction("PostDetails", "Home", new { postId });
        }

        public async Task<IActionResult> EditComment(string content, string userId, string commentId, string postId)
        {
            string contentEncoded = HttpUtility.UrlEncode(content);

            var response = await _httpClient
                .PutAsync($"api/Post/Comment/{commentId}?userId={userId}&content={contentEncoded}", null);

            if(!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al publicar, intentelo más tarde.";
            }

            return RedirectToAction("PostDetails", "Home", new { postId });
        }

        public async Task<IActionResult> ReplyComment(string commentId, string content, string userId, string postId)
        {

            string contentEncoded = HttpUtility.UrlEncode(content);

            var response = await 
                _httpClient.PostAsync($"api/Post/Reply/{commentId}?content={contentEncoded}&userId={userId}", null);

            

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("PostDetails",  "Home", new { postId }); 
            }
            else
            {
                TempData["ErrorMessage"] = "Error al publicar, intentelo más tarde.";

                return RedirectToAction("PostDetails", "Home", new { postId });
            }
        }
        public async Task<IActionResult> PinComment(string userId, string postId, string commentId)
        {
            var response = await _httpClient
                .PutAsync($"api/Post/PinComment/{commentId}?userId={userId}&postId={postId}", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("PostDetails", "Home", new { postId });
            }
            else
            {
                TempData["ErrorMessage"] = "Error al fijar comentario, intentelo más tarde.";

                return RedirectToAction("PostDetails", "Home", new { postId });
            }

        }

        public async Task<IActionResult> LikeComment(string userId, string commentId, string isDislike, string postId)
        {
            var response = await _httpClient
                .PutAsync($"api/Post/Like/{commentId}?userId={userId}&isDislike_={isDislike}", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("PostDetails", "Home", new { postId });
            }
            else
            {

                TempData["ErrorMessage"] = "Error al dar me gusta al comentario, intentelo más tarde.";

                return RedirectToAction("PostDetails", "Home", new { postId });
            }
        }

        public async Task<IActionResult> DeleteComment(string commentId, string postId, string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/Post/Comment/{commentId}?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("PostDetails", "Home", new { postId });
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el comentario, intentelo más tarde.";

                return RedirectToAction("PostDetails", "Home", new { postId });
            }
        }

        public async Task<IActionResult> DeletePost(string postDeleteId, string postId, string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/Post/{postDeleteId}?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "El posteo se ha eliminado exitosamente.";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el posteo, intentelo más tarde.";

                return RedirectToAction("PostDetails", "Home", new { postId });
            }
        }

        public IActionResult NewPost()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
