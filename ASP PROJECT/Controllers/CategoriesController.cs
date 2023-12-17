using ASP_PROJECT.Data;
using Microsoft.AspNetCore.Mvc;

//REMINDER: Din ultima cerinta deducem ca adminul NU STERGE CATEGORIILE USERILOR
namespace ASP_PROJECT.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db;
        public CategoriesController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var categories = from category in db.Categories
                             orderby category.Name
                             select category;
            ViewBag.Categories = categories;
            return View();
        }
    }
}
