using BlogApp.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Xml.Linq;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var connectionString = "Server=104.247.162.242\\MSSQLSERVER2019;Initial Catalog=miraykar_blog;User Id=miraykar_blogdbuser;Password=;TrustServerCertificate=True";

            using var connection = new SqlConnection(connectionString);
            var sql = "SELECT * FROM posts ORDER BY updated_date DESC";
            var posts = connection.Query<Post>(sql).ToList();

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

        public IActionResult Detay(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("index");
            }

            var postModel = new PostModel();

            var connectionString = "Server=104.247.162.242\\MSSQLSERVER2019;Initial Catalog=miraykar_blog;User Id=miraykar_blogdbuser;Password=;TrustServerCertificate=True";

            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT * FROM posts WHERE id = @id";
                var post = connection.QuerySingleOrDefault<Post>(sql, new { id = id });
                postModel.Post = post;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT * FROM comments WHERE PostId = @id AND IsApproved = 1";
                var comments = connection.Query<PostComment>(sql, new { id = id }).ToList();
                postModel.Comments = comments;
            }
           
            return View(postModel);
        }

        [HttpPost]
        public IActionResult YorumEkle(PostComment model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            model.CreatedDate = DateTime.Now;

            var connectionString = "Server=104.247.162.242\\MSSQLSERVER2019;Initial Catalog=miraykar_blog;User Id=miraykar_blogdbuser;Password=;TrustServerCertificate=True";

            using var connection = new SqlConnection(connectionString);

            var sql = "INSERT INTO comments (Name, Comment, PostId, CreatedDate) VALUES (@Name, @Comment, @PostId, @CreatedDate)";

            try
            {
                var affectedRows = connection.Execute(sql, model);

                //ViewBag.MessageCssClass = "alert-success";
                //ViewBag.Message = "Yorum gönderilddi, onay için bekliyor.";
                //return View("Message", new { id = model.PostId });

                return RedirectToAction("Detay", new { id = model.PostId });
            }
            catch
            {
                return RedirectToAction("Index");
            }            
        }

        public IActionResult Foto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Foto(FotoModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Detay");
            }

            var ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);

            var path =Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", ImageName);

            using var stream = new FileStream(path, FileMode.Create);
            model.Image.CopyTo(stream);

            ViewBag.Image = $"/uploads/{ImageName}";
            return View();
        }
    }
}
