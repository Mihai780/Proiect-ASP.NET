using ASP_PROJECT.Data;
using ASP_PROJECT.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_PROJECT.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext db;
        public CommentsController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpPost]
        public IActionResult New(Comment comm)
        {
            comm.Date = DateTime.Now;

            try
            {
                db.Comments.Add(comm);
                db.SaveChanges();
                return Redirect("/Bookmarks/Show/" + comm.BookmarkId);
            }

            catch (Exception)
            {
                return Redirect("/Bookmarks/Show/" + comm.BookmarkId);
            }

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);
            db.Comments.Remove(comm);
            db.SaveChanges();
            return Redirect("/Bookmarks/Show/" + comm.BookmarkId);
        }

        public IActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);
            ViewBag.Comment = comm;
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id, Comment requestComment)
        {
            Comment comm = db.Comments.Find(id);
            try
            {

                comm.Content = requestComment.Content;

                db.SaveChanges();

                return Redirect("/Bookmarks/Show/" + comm.BookmarkId);
            }
            catch (Exception e)
            {
                return Redirect("/Bookmarks/Show/" + comm.BookmarkId);
            }

        }
    }
}
