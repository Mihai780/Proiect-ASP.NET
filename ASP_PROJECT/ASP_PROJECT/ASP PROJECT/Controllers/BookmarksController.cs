using ASP_PROJECT.Data;
using ASP_PROJECT.Models;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

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

        [Authorize(Roles ="User,Admin")]
        public IActionResult LikeUD(int id, string userid)
        {
            Bookmark bookmark = db.Bookmarks.Include("User")
                               .Include("Comments")
                               .Include("Comments.User")
                               .Where(bok => bok.Id == id)
                               .First();
            if (User.IsInRole("User") || User.IsInRole("Admin"))
            {
                if (TempData.ContainsKey(id.ToString() + userid))
                {
                    bookmark.Likes--;
                    TempData.Remove(id.ToString() + userid);
                }

                else
                {
                    bookmark.Likes++;
                    TempData[id.ToString() + userid] = "Liked";
                }
                db.SaveChanges();
            }
            
            ViewBag.message = "Buna";
            
            return Redirect("/Bookmarks/Show/" + id);
        }
        public IActionResult DateOrLike(int id)
        {
            var bookmarks = from bookmark in db.Bookmarks.Include("User") select bookmark;

            if (id == 1 || (TempData.ContainsKey("D") && id!=2))
            {
                bookmarks = from bookmark in db.Bookmarks.Include("User")
                            orderby bookmark.Date descending
                            select bookmark;
                TempData["D"] = 1;
                TempData.Remove("L");
                ViewBag.NumeSite = "Postari Recente";
            }
            else
            {

                bookmarks = from bookmark in db.Bookmarks.Include("User")
                            orderby bookmark.Likes descending
                            select bookmark;
                TempData["L"] = 1;
                TempData.Remove("D");
                ViewBag.NumeSite = "Postari Populare";
            }

            var search = "";
            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();
                List<int> bookmarkID = db.Bookmarks.Where(
                                     bm => bm.Title.Contains(search) || bm.Description.Contains(search)
                                      ).Select(bm => bm.Id).ToList();

                List<int> CategoryID = db.Categories.Include("BookmarkCategories").Where(
                                        cat=>cat.CategoryName.Contains(search)
                                        ).Select(cat=>cat.Id).ToList();

                List<int> categoryID = db.BookmarkCategories.Include("Category.User").Where(
                        cat => cat.Category.CategoryName.Contains(search) && cat.Category.UserId == _userManager.GetUserId(User)
                        && cat.Category != null
                        ).Select(cat => (int)cat.BookmarkId).ToList();

                List<int> mergedIds = bookmarkID.Union(categoryID).ToList();

                bookmarks = db.Bookmarks.Where(bookmark => mergedIds.Contains(bookmark.Id))
                               .Include("User")
                              .OrderBy(b => b.Title);
                ViewBag.Bookmarks = bookmarks;
       
            }
            ViewBag.SearchString = search;

            int _perPage = 10;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.alert = TempData["messagetype"];
            }


            int totalItems = bookmarks.Count();


            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            var paginatedbookmarks = bookmarks.Skip(offset).Take(_perPage);

            ViewBag.Nr = paginatedbookmarks.Count();

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            ViewBag.Bookmarks = paginatedbookmarks;

            return View();
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.messageType = TempData["messageType"].ToString();
            }

            var bookmarksdate = (from bookmark in db.Bookmarks.Include("User")
                                 orderby bookmark.Date descending
                                 select bookmark).Take(5);
            var bookmarkslike = (from bookmark in db.Bookmarks.Include("User")
                                 orderby bookmark.Likes descending
                                 select bookmark).Take(5);


            var search = "";
            if (Convert.ToString(HttpContext.Request.Query["search"]) !=null)
            {
                search =Convert.ToString(HttpContext.Request.Query["search"]).Trim();
                List<int> bookmarkID = db.Bookmarks.Where(
                                     bm => bm.Title.Contains(search) || bm.Content.Contains(search) || bm.Description.Contains(search)
                                      ).Select(bm=>bm.Id).ToList();

                List<int> categoryID = db.BookmarkCategories.Include("Category.User").Where(
                                        cat => cat.Category.CategoryName.Contains(search) && cat.Category.UserId == _userManager.GetUserId(User)
                                        && cat.Category!=null
                                        ).Select(cat => (int)cat.BookmarkId).ToList();

                List<int> mergedIds = bookmarkID.Union(categoryID).ToList();

                var bookmarks = db.Bookmarks.Where(bookmark => mergedIds.Contains(bookmark.Id))
                               .Include("User")
                              .OrderBy(b => b.Title);
                ViewBag.Bookmarks = bookmarks;
                return View();         
            }
            ViewBag.SearchString = search;
            if (search != "")
            {
                ViewBag.PaginationBaseUrl = "/Articles/Index/?search="
                + search;
               
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/Articles/Index"; 
            }
            ViewBag.BookmarksDate = bookmarksdate;
            ViewBag.BookmarksLike = bookmarkslike;
            return View();
        }

        public IActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.messageType = TempData["messageType"].ToString();
            }
            Bookmark bookmark = db.Bookmarks.Include("User")
                                .Include("Comments")
                                .Include("Comments.User")
                                .Where(bok => bok.Id == id)
                                .First();

            ViewBag.categories = db.Categories.Where(b => b.UserId == _userManager.GetUserId(User))
                                .ToList();

            SetAccessRights();


            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }

            return View(bookmark);
        }

        [HttpPost]
        [Authorize(Roles ="User,Admin")]
        public IActionResult AddCategory([FromForm] BookmarkCategory bookmarkCategory)
        {
            if (ModelState.IsValid)
            {
                if (db.BookmarkCategories
                   .Where(ab => ab.BookmarkId == bookmarkCategory.BookmarkId)
                   .Where(ab => ab.CategoryId == bookmarkCategory.CategoryId)
                   .Count() > 0)
                {
                    TempData["message"] = "Acest bookmark este deja adaugat in categoria respectiva";
                    TempData["messageType"] = "alert-danger";
                }
                else
                {
                    db.BookmarkCategories.Add(bookmarkCategory);
                    db.SaveChanges();

                    TempData["message"] = "Bookmark-ul a fost adaugat in categoria selectata";
                    TempData["messageType"] = "alert-success";
                }

            }
            else
            {
                TempData["message"] = "Nu s-a putut adauga bookmarkul";
                TempData["messageType"] = "alert-danger";
            }
            return Redirect("/Bookmarks/Show/" + bookmarkCategory.BookmarkId);
        }
        

        [HttpPost]
        [Authorize(Roles ="User,Admin")]
        public IActionResult Show([FromForm] Comment comm)
        {
            comm.Date = DateTime.Now;
            comm.UserId = _userManager.GetUserId(User);

            if(ModelState.IsValid)
            {
                db.Comments.Add(comm);
                db.SaveChanges();
                return Redirect("/Bookmarks/Show/" + comm.BookmarkId);
            }
            else
            {
                Bookmark bookmark = db.Bookmarks.Include("User")
                                .Include("Comments")
                                .Include("Comments.User")
                                .Where(bok => bok.Id == comm.BookmarkId)
                                .First();


                SetAccessRights();

                return View(bookmark);
            }

        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult New()
        {
            Bookmark bm = new Bookmark();

            bm.Categ = GetUserCategories();

            return View(bm);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult New(Bookmark bookmark)
        {
            var sanitizer = new HtmlSanitizer();
            if (ModelState.IsValid)
            {

                //bookmark.Content = sanitizer.Sanitize(bookmark.Content);
                //bookmark.Content = (bookmark.Content);    nu afiseaza

                bookmark.UserId = _userManager.GetUserId(User);
                bookmark.Date = DateTime.Now;
                bookmark.Likes = 0;
                db.Bookmarks.Add(bookmark);
                db.SaveChanges();
                TempData["message"] = "Bookmarkul a fost adaugat";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index"); 
            }

            else
            {
                bookmark.Categ = GetUserCategories();
                return View(bookmark);
            }
        }

        [Authorize(Roles ="User,Admin")]
        public IActionResult Edit(int id)
        {
            Bookmark bookmark = db.Bookmarks.Where(bok => bok.Id==id).First();

            bookmark.Categ = GetUserCategories();
            if (bookmark.UserId==_userManager.GetUserId(User) || User.IsInRole("Admin"))
                return View(bookmark);
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui bookmark care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Edit(int id,Bookmark requestBookmark)
        {
            Bookmark bookmark = db.Bookmarks.Find(id);

            if (bookmark.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {

                    bookmark.Title = requestBookmark.Title;
                    bookmark.Description = requestBookmark.Description;
                    bookmark.Content = requestBookmark.Content;
                    db.SaveChanges();
                    TempData["message"] = "Bookmarkul a fost modificat";
                    TempData["messageType"] = "alert-success";
                    return RedirectToAction("Index");
                }

                else
                {
                    requestBookmark.Categ = GetUserCategories();
                    return RedirectToAction("Edit", id);

                }
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui bookmark care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Bookmark bookmark = db.Bookmarks.Include("Comments")
                                         .Where(bok => bok.Id == id)
                                         .First();

            if (bookmark.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                if (bookmark.Comments.Count>0)
                {
                    foreach (var comment in bookmark.Comments)
                    {
                        db.Comments.Remove(comment);
                    }
                }

                db.Bookmarks.Remove(bookmark);
                db.SaveChanges();
                TempData["message"] = "Bookmarul a fost sters";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un bookmark care nu va apartine";
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

            var selectList = new List<SelectListItem>();

            var categories = from cat in db.Categories
                             where cat.UserId==_userManager.GetUserId(User)
                             select cat;

            foreach (var category in categories)
            {
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
