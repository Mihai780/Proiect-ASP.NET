using ASP_PROJECT.Data;
using ASP_PROJECT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//REMINDER: Din ultima cerinta deducem ca adminul NU STERGE CATEGORIILE USERILOR
namespace ASP_PROJECT.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public CategoriesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }

        [Authorize(Roles ="User,Admin")]
        public IActionResult Index()
        {

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.messageType = TempData["messageType"].ToString();
            }

            if (User.IsInRole("Admin"))
            {
                var categories = from category in db.Categories.Include("User")
                                 orderby category.CategoryName
                                 select category;
                ViewBag.Categories = categories;
               
            }
            else if (User.IsInRole("User"))
            {
                var categories = from category in db.Categories.Include("User")
                                 .Where(cat=>cat.UserId == _userManager.GetUserId(User))
                                 orderby category.CategoryName
                                 select category;
                ViewBag.Categories = categories;
                
            }
            return View();
            
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Show(int id)
        {
            SetAccessRights();
            Category category = db.Categories.Find(id);
            if (User.IsInRole("User"))
            {
                var categories = db.Categories
                                .Include("BookmarkCategories.Bookmark.User")
                                .Include("User")
                                .Where(b => b.Id == id)
                                .Where(b => b.UserId == _userManager.GetUserId(User))
                                .FirstOrDefault();

                if (categories == null)
                {
                    TempData["message"] = "Nu aveti drepturi";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Bookmarks");
                }

                return View(categories);
            }
            else
            if (User.IsInRole("Admin"))
            {
                var categories = db.Categories
                                .Include("BookmarkCategories.Bookmark.User")
                                .Include("User")
                                .Where(b => b.Id == id)
                                .FirstOrDefault();

                if (categories == null)
                {
                    TempData["message"] = "Resursa cautata nu exista";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Bookmarks");
                }

                return View(categories);
            }

            else 
            {
                TempData["message"] = "Trebuie sa fiti logat pentru a vedea aceasta pagina";
                TempData["messageType"] = "alert-danger";
                return Redirect("/Bookmarks/Index");
            }
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult New()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult New(Category cat)
        {
            if (ModelState.IsValid)
            {
                cat.UserId=_userManager.GetUserId(User);
                db.Categories.Add(cat);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }

            else
            {
                return View(cat);
            }
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id)
        {
            
            Category category = db.Categories.Find(id);
            if (category.UserId==_userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(category);
            }
            else
            {
                TempData["message"] = "Categoria pe care ati incercat sa o editati nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
            
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Category requestCategory)
        {
            Category category = db.Categories.Find(id);
            if (category.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {

                    category.CategoryName = requestCategory.CategoryName;
                    category.Description = requestCategory.Description;
                    db.SaveChanges();
                    TempData["message"] = "Categoria a fost modificata!";
                    TempData["messageType"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requestCategory);
                }
            }         
            else
            {
                TempData["message"] = "Categoria pe care ati incercat sa o editati nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        
        public IActionResult Delete(int id)
        {
            Category category= db.Categories.Include("BookmarkCategories")
                                         .Where(cat => cat.Id == id)
                                         .First();
            if (category.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                if (category.BookmarkCategories.Count>0)
                {
                    foreach (var bmcat in category.BookmarkCategories)
                        db.BookmarkCategories.Remove(bmcat);
                }
                db.Categories.Remove(category);
                TempData["message"] = "Categoria a fost stearsa";
                TempData["messageType"] = "alert-success";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Categoria pe care ati incercat sa o stergeti nu va apartine";
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
    }
}
