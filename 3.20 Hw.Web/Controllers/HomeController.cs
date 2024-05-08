using _3._20_Hw.Data;
using _3._20_Hw.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace _3._20_Hw.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var repo = new ImageRepository(_connectionString);
            return View(new ImageViewModel
            {
                Images = repo.GetImages()
            });
        }
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(IFormFile image, string title)
        {
            var fileName = $"{Guid.NewGuid()}-{image.FileName}";
            var fullImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using FileStream fs = new FileStream(fullImagePath, FileMode.Create);
            image.CopyTo(fs);
            var repo = new ImageRepository(_connectionString);
            repo.AddImage(new Images
            {
                Title = title,
                Name = fileName,
                Date = DateTime.Now
            });
            return Redirect("/");
        }
        public IActionResult ViewImage(int id)
        {
            var repo = new ImageRepository(_connectionString);
            var image = repo.GetImageById(id);
            var vm = new ImageViewModel
            {
                Image = image,
                CanLike = true 
            };
            List<int> likedIds = HttpContext.Session.Get<List<int>>("LikedIds");

            if (likedIds == null)
            {
                likedIds = new List<int>();
            }

            if (likedIds.Contains(id))
            {
                vm.CanLike = false;
            }
            else
            {
                likedIds.Add(id);
                HttpContext.Session.Set("LikedIds", likedIds); 
            }

            return View(vm);
        }

        [HttpPost]
        public void AddLikes(int id)
        {
            var repo = new ImageRepository(_connectionString);
            repo.AddLikes(id);
           

        }
        public IActionResult GetLikes(int id)
        {
            var repo = new ImageRepository(_connectionString);
            var likes = repo.GetLikes(id);
            return Json(likes);
        }

    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}