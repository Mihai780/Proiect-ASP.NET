using ASP_PROJECT.Data;
using ASP_PROJECT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_PROJECT.Controllers
{
    public class BookmarksController : Controller
    {
        private readonly ApplicationDbContext db;
        public BookmarksController(ApplicationDbContext context)
        {
            db = context;
        }


        public IActionResult Index()
        {
            var bookmarks = db.Bookmarks;

            ViewBag.Bookmarks = bookmarks;

            return View();
        }

        public IActionResult Show(int id)
        {
            Bookmark bookmark = db.Bookmarks.Include("Comments").Where(bok => bok.Id == id).First();

            ViewBag.Bookmark = bookmark;

            return View();
        }

        public IActionResult New()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult New(Bookmark bookmark)
        {
            try
            {
                bookmark.Date = DateTime.Now;
                db.Bookmarks.Add(bookmark);
                db.SaveChanges();
                TempData["message"] = "Bookmarkul a fost adaugat";
                return RedirectToAction("Index"); 
            }

            catch (Exception)
            {
                return RedirectToAction("New");
            }
        }

        public IActionResult Edit(int id)
        {
            Bookmark bookmark = db.Bookmarks.Where(bok => bok.Id==id).First();

            ViewBag.Bookmark = bookmark;

            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id,Bookmark requestBookmark)
        {
            Bookmark bookmark = db.Bookmarks.Find(id);

            try
            {

                bookmark.Title = requestBookmark.Title;
                bookmark.Description = requestBookmark.Description;
                bookmark.Content = requestBookmark.Content;
                db.SaveChanges();
                TempData["message"] = "Bookmarkul a fost modificat";
                return RedirectToAction("Index");
            }

            catch (Exception)
            {
                return RedirectToAction("Edit", id);

            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Bookmark article = db.Bookmarks.Find(id);
            db.Bookmarks.Remove(article);
            db.SaveChanges();
            TempData["message"] = "Bookmarul a fost sters";
            return RedirectToAction("Index");
        }
    }
}
