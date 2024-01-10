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

       [StringLength(20, ErrorMessage = "Username-ul trebuie sa fie intre 3 si 20 de caractere!")]
       [MinLength(3, ErrorMessage="Username-ul trebuie sa fie intre 3 si 20 de caractere!")]
        public string? Nickname { get; set; }
        public byte[]? ProfilePic { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }
    }

}
