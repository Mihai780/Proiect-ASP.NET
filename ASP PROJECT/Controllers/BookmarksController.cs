using ASP_PROJECT.Data;
using ASP_PROJECT.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            Bookmark bm = new Bookmark();

            bm.Categ = GetUserCategories();

            return View(bm);
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
                bookmark.Categ = GetUserCategories();
                return RedirectToAction("New");
            }
        }

        public IActionResult Edit(int id)
        {
            Bookmark bookmark = db.Bookmarks.Where(bok => bok.Id==id).First();

            bookmark.Categ = GetUserCategories();

            return View(bookmark);
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
                requestBookmark.Categ = GetUserCategories();
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

        [NonAction]
        public IEnumerable<SelectListItem> GetUserCategories()
        {
            // generam o lista de tipul SelectListItem fara elemente
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                // id-ul categoriei si denumirea acesteia
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            return selectList;
        }
    }
}
