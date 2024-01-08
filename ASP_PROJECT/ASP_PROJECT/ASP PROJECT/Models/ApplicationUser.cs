using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ASP_PROJECT.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Bookmark>? Bookmarks { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(25, ErrorMessage = "Username must not be longer than 15 charcters")]
        [MinLength(3, ErrorMessage = "Username-ul trebuie sa aiba 3 caractere")]
        public string Username { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }
    }

}
