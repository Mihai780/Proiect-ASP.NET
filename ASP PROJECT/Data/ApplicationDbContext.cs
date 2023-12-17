using ASP_PROJECT.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP_PROJECT.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet <BookmarkCategory> BookmarkCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // definirea relatiei many-to-many dintre Article si Bookmark

            base.OnModelCreating(modelBuilder);

            // definire primary key compus
            modelBuilder.Entity<BookmarkCategory>()
                .HasKey(ab => new { ab.Id, ab.BookmarkId, ab.CategoryId });


            // definire relatii cu modelele Bookmark si Article (FK)

            modelBuilder.Entity<BookmarkCategory>()
                .HasOne(ab => ab.Bookmark)
                .WithMany(ab => ab.BookmarkCategories)
                .HasForeignKey(ab => ab.BookmarkId);

            modelBuilder.Entity<BookmarkCategory>()
                .HasOne(ab => ab.Category)
                .WithMany(ab => ab.BookmarkCategories)
                .HasForeignKey(ab => ab.CategoryId);
        }
    }
}