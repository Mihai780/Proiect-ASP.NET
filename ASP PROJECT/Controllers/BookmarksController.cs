using ASP_PROJECT.Data;
using ASP_PROJECT.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP_PROJECT.Controllers
{
    public class BookmarksController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public BookmarksController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var bookmarks = db.Bookmarks.Include("User");

            ViewBag.Bookmarks = bookmarks;

            return View();
        }

        public IActionResult Show(int id)
        {
            Bookmark bookmark = db.Bookmarks.Include("User").Include("Comments").Include("Comments.User").Where(bok => bok.Id == id).First();

            ViewBag.Bookmark = bookmark;

            SetAccessRights();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }

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
                bookmark.UserId = _userManager.GetUserId(User);
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

            if (bookmark.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
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

            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Bookmark bookmark = db.Bookmarks.Find(id);

            if (bookmark.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Bookmarks.Remove(bookmark);
                db.SaveChanges();
                TempData["message"] = "Bookmarul a fost sters";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un articol care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
            
            
        }

        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("User"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.UserCurent = _userManager.GetUserId(User);
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
