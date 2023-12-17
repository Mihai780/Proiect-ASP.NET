using System.ComponentModel.DataAnnotations;

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

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
