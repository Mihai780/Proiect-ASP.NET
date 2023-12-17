using Microsoft.AspNetCore.Identity;

namespace ASP_PROJECT.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Bookmark>? Bookmarks { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }
    }

}
