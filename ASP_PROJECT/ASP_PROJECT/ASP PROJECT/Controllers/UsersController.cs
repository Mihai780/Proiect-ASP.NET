using ASP_PROJECT.Data;
using ASP_PROJECT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP_PROJECT.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var users = from user in db.Users
                            orderby user.UserName
                            select user;
                ViewBag.UsersList = users;
                return View();
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul la resursa aceasta";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("/Bookmarks/Index");
            }

            
        }

        public async Task<ActionResult> Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            SetAccesRights();
            if (User.IsInRole("Admin"))
            {
                var roles = await _userManager.GetRolesAsync(user);

                ViewBag.Roles = roles;
            }
            else
            {
                ViewBag.Roles = null;
            }
            var userposts = db.Users
                          .Include("Categories")
                          .Include("Categories.BookmarkCategories")
                          .Include("Categories.BookmarkCategories.Bookmark.User")
                          .Where(u => u.Id == id)
                          .FirstOrDefault();

            List<Bookmark> userbookmarks=new List<Bookmark>();

            foreach (var categ in userposts.Categories)
                foreach (var bmcat in categ.BookmarkCategories)
                    if (!userbookmarks.Contains(bmcat.Bookmark))
                        userbookmarks.Add(bmcat.Bookmark);

            ViewBag.UserPosts=userbookmarks;

            return View(userposts);
        }

        [Authorize(Roles ="Admin,User")]
        public async Task<ActionResult> Edit(string id)
        {
            if (User.IsInRole("Admin") || _userManager.GetUserId(User).ToString()==id)
            {
                ApplicationUser user = db.Users.Find(id);
                if (User.IsInRole("Admin"))
                {
                    user.AllRoles = GetAllRoles();

                    var roleNames = await _userManager.GetRolesAsync(user); // Lista de nume de roluri

                    // Cautam ID-ul rolului in baza de date
                    var currentUserRole = _roleManager.Roles
                                                      .Where(r => roleNames.Contains(r.Name))
                                                      .Select(r => r.Id)
                                                      .First(); // Selectam 1 singur rol
                    ViewBag.UserRole = currentUserRole;
                }
               
                return View(user);
            }        
            else
            {
                TempData["message"] = "Nu aveti dreptul la resursa aceasta";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("/Bookmarks/Index");
            }
        }

        [Authorize(Roles ="Admin, User")]
        [HttpPost]
        public async Task<ActionResult> Edit(string id, ApplicationUser newData, [FromForm] string newRole)
        {
            ApplicationUser user = db.Users.Find(id);

            user.AllRoles = GetAllRoles();

            if (User.IsInRole("Admin") || _userManager.GetUserId(User).ToString() == id)
            {
                if (ModelState.IsValid)
                {
                    user.UserName = newData.UserName;
                    user.Email = newData.Email;
                    user.FirstName = newData.FirstName;
                    user.LastName = newData.LastName;
                    user.PhoneNumber = newData.PhoneNumber;
                    user.Nickname = newData.Nickname;


                    if (User.IsInRole("Admin"))
                    {
                        var roles = db.Roles.ToList();

                        foreach (var role in roles)
                        {
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                        var roleName = await _roleManager.FindByIdAsync(newRole);
                        await _userManager.AddToRoleAsync(user, roleName.ToString());
                    }


                    db.SaveChanges();
                    TempData["message"] = "Utilizator sters";
                    TempData["messageType"] = "alert-success";

                }
                if (User.IsInRole("Admin"))
                    return Redirect("/Users/Index");
                else
                    return Redirect("/Users/Show/" + id);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul la resursa aceasta";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("/Bookmarks/Index");
            }

            
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (User.IsInRole("Admin") && _userManager.GetUserId(User).ToString() != id)
            {
                var user = db.Users
                        .Include("Bookmarks")
                        .Include("Comments")
                        .Include("Categories")
                        .Where(u => u.Id == id)
                        .First();

                // Delete user comments
                if (user.Comments.Count > 0)
                {
                    foreach (var comment in user.Comments)
                    {
                        db.Comments.Remove(comment);
                    }
                }

                // Delete user bookmarks
                if (user.Bookmarks.Count > 0)
                {
                    foreach (var bookmark in user.Bookmarks)
                    {
                        db.Bookmarks.Remove(bookmark);
                    }
                }

                // Delete user categories
                if (user.Categories.Count > 0)
                {
                    foreach (var category in user.Categories)
                    {
                        db.Categories.Remove(category);
                    }
                }

                db.ApplicationUsers.Remove(user);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else if (User.IsInRole("Admin") && _userManager.GetUserId(User).ToString() == id)
            {
                TempData["message"] = "Nu cred ca ai vrea sa te stergi singur";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul la resursa aceasta";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("/Bookmarks/Index");
            }
           
        }


        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in db.Roles
                        select role;

            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }

        public virtual void SetAccesRights()
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
