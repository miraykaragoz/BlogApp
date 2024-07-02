using BlogApp.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace BlogApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var connectionString = "Server=104.247.162.242\\MSSQLSERVER2019;Initial Catalog=miraykar_blog;User Id=miraykar_blogdbuser;Password=;TrustServerCertificate=True";

            using var connection = new SqlConnection(connectionString);
            var posts = connection.Query<Post>("SELECT id, title, created_date as CreatedDate, updated_date as UpdatedDate FROM posts").ToList();

            return View(posts);
        }

        public IActionResult Duzenle(int id)
        {
            var connectionString = "Server=104.247.162.242\\MSSQLSERVER2019;Initial Catalog=miraykar_blog;User Id=miraykar_blogdbuser;Password=;TrustServerCertificate=True";

            using var connection = new SqlConnection(connectionString);

            var post = connection.QuerySingleOrDefault<Post>("SELECT * FROM posts WHERE id = @Id", new { Id = id });

            return View(post);
        }

        [HttpPost]
        public IActionResult Duzenle(Post model)
        {
            var connectionString = "Server=104.247.162.242\\MSSQLSERVER2019;Initial Catalog=miraykar_blog;User Id=miraykar_blogdbuser;Password=;TrustServerCertificate=True";

            using var connection = new SqlConnection(connectionString);

            var sql = "UPDATE posts SET title = @Title, summary = @Summary, detail = @Detail, updated_date = @UpdatedDate WHERE id = @Id";
            var param = new{
                Title  = model.Title,
                Summary = model.Summary,
                Detail = model.Detail,
                UpdatedDate = DateTime.Now,
                Id = model.Id 
            };

            var rowsAffected = connection.Execute(sql, param);

            ViewBag.Message = "Post güncelleme işlemi başarıyla gerçekleşti.";
            ViewBag.MessageCssClass = "alert-success";
            return View("message");
        }

        public IActionResult sil(int id)
        {
            var connectionString = "Server=104.247.162.242\\MSSQLSERVER2019;Initial Catalog=miraykar_blog;User Id=miraykar_blogdbuser;Password=;TrustServerCertificate=True";

            using var connection = new SqlConnection(connectionString);

            var sql = "DELETE FROM posts WHERE id = @Id";

            var rowsAffected = connection.Execute(sql, new {Id = id});

            return RedirectToAction("Index");
        }

        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Post model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MessageCssClass = "alert-danger";
                ViewBag.Message = "Post ekleme işlemi başarısız oldu.";
                return View("Message");
            }

            model.CreatedDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;

            var connectionString = "Server=104.247.162.242\\MSSQLSERVER2019;Initial Catalog=miraykar_blog;User Id=miraykar_blogdbuser;Password=;TrustServerCertificate=True";

            using var connection = new SqlConnection(connectionString);

            var sql = "INSERT INTO posts (title, summary, detail, created_date, updated_date) VALUES (@Title, @Summary, @Detail, @CreatedDate, @UpdatedDate)";

            var data = new
            {
                model.Title,
                model.Summary,
                model.Detail,
                model.CreatedDate,
                model.UpdatedDate
            };

            var rowsAffected = connection.Execute(sql, data);

            ViewBag.MessageCssClass = "alert-success";
            ViewBag.Message = "Post ekleme işlemi başarıyla gerçekleşti.";
            return View("Message");
        }

        public IActionResult Yorumlar()
        {
            var connectionString = "Server=104.247.162.242\\MSSQLSERVER2019;Initial Catalog=miraykar_blog;User Id=miraykar_blogdbuser;Password=;TrustServerCertificate=True";

            using var connection = new SqlConnection(connectionString);

            var sql = "SELECT * FROM comments ORDER BY CreatedDate DESC";
            var comments = connection.Query<PostComment>(sql).ToList();

            return View(comments);
        }

        [HttpPost]
        public IActionResult YorumOnayla(PostComment model)
        {
            var connectionString = "Server=104.247.162.242\\MSSQLSERVER2019;Initial Catalog=miraykar_blog;User Id=miraykar_blogdbuser;Password=;TrustServerCertificate=True";

            using var connection = new SqlConnection(connectionString);

            var sql = "UPDATE comments SET IsApproved = 1 WHERE IsApproved = 0 AND Id = @Id";

            var parameters = new { Id = model.Id };

            connection.Execute(sql, parameters);

            ViewBag.MessageCssClass = "alert-success";
            ViewBag.Message = "Yorum onaylama işlemi başarıyla gerçekleşti.";
            return View("Message");
        }

        [HttpPost]
        public IActionResult YorumSil(int id)
        {
            var connectionString = "Server=104.247.162.242\\MSSQLSERVER2019;Initial Catalog=miraykar_blog;User Id=miraykar_blogdbuser;Password=;TrustServerCertificate=True";

            using var connection = new SqlConnection(connectionString);
            
            //tek parametre gönderildiği için @id yerine  sql = "" + id yazdım

            var sql = "DELETE FROM comments WHERE Id = " + id;

            var rowsAffected = connection.Execute(sql, new { Id = id });

            ViewBag.MessageCssClass = "alert-success";
            ViewBag.Message = "Yorum silme işlemi başarıyla gerçekleşti.";
            return View("Message");
        }
    }
}
