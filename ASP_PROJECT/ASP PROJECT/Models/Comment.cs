using System.ComponentModel.DataAnnotations;

namespace ASP_PROJECT.Models
{
        public class Comment
        {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "Continutul este obligatoriu")]
            public string Content { get; set; }

            public DateTime Date { get; set; }

            public int? BookmarkId { get; set; }

            public virtual Bookmark? Bookmark { get; set; }
            public string? UserId { get; set; }           
            public virtual ApplicationUser? User { get; set; }
        }
}
