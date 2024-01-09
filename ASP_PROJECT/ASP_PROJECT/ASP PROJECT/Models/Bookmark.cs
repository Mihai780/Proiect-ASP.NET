using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_PROJECT.Models
{
    public class Bookmark
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Titlul este obligatoriu")]
        public string Title { get; set; }

        [Required(ErrorMessage ="Descrierea este obligatorie")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Continutul trebuie sa existe")]
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public int? CategoryId { get; set; }
        
        public int Likes { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        
        public virtual ICollection<BookmarkCategory>? BookmarkCategories { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

    }
}
