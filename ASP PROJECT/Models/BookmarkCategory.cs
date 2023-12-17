using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_PROJECT.Models
{
    public class BookmarkCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // cheie primara compusa (Id, ArticleId, BookmarkId)
        public int Id { get; set; }
        public int? BookmarkId { get; set; }
        public int? CategoryId { get; set; }
        public virtual Bookmark? Bookmark { get; set; }
        public virtual Category? Category { get; set; }
        public DateTime BookmarkDate { get; set; }
    }
}
