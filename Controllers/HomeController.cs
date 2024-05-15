using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            ViewData["title"] = "Anasayfa";

            var posts = new List<Post>();
            posts.Add(
                new Post()
                {
                    Title = "Ba�l�k",
                    Summary = "�zet",
                    ImgUrl = "https://source.unsplash.com/collection/1346951/1000x500?sig=1"
                }
            );

            posts.Add(
                new Post()
                {
                    Title = " Di�er Ba�l�k",
                    Summary = "Di�er �zet",
                    ImgUrl = "https://source.unsplash.com/collection/1346951/1000x500?sig=1"
                }
            );

            return View(posts);
        }

        public IActionResult Hakkimda()
        {
            return View();
        }

        public IActionResult Iletisim()
        {
            return View();
        }

        public IActionResult Detay()
        {
            var postDetail = new Post();
            //postDetail.Title = "";
            postDetail.Content = "<p class\"pb-3\"> uzun i�erik gibi sanki </p>";
            postDetail.ImgUrl = "https://source.unsplash.com/collection/1346951/1000x500?sig=1";

            ViewData["title"] = "G�nderi Detay";
            return View(postDetail);
        }        
    }
}
