using System.ComponentModel.DataAnnotations;

namespace ASP_PROJECT.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele categoriei este obligatoriu")]
        public string CategoryName { get; set; }

        [MaxLength(20, ErrorMessage = "Descrierea nu trebuie sa depaseasca 20 de caractere")]
        public string Description { get; set; }

        // o colectie este creata de catre un user
        //public string? UserId { get; set; }
        //public virtual ApplicationUser? User { get; set; }

        // relatia many-to-many dintre Article si Bookmark
        public virtual ICollection<BookmarkCategory>? BookmarkCategories { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
