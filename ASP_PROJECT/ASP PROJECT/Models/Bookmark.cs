using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_PROJECT.Models
{
    public class Bookmark
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
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
