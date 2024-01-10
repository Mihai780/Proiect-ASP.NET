using ASP_PROJECT.Data;
using ASP_PROJECT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ASP_PROJECT.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService
                <DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Roles.Any())
                {
                    return;  
                }

                context.Roles.AddRange(
                    new IdentityRole { Id = "47991959-d03c-4c41-8938-83cf42af1067", Name = "Admin", NormalizedName = "Admin".ToUpper() },
                    new IdentityRole { Id = "47991959-d03c-4c41-8938-83cf42af1068", Name = "User", NormalizedName = "User".ToUpper() }
                );

                var hasher = new PasswordHasher<ApplicationUser>();

                context.Users.AddRange(
                    new ApplicationUser
                    {
                        Id = "3ce4f0e3-2476-4092-9209-4cce498cefa1", 
                        UserName = "admin@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "ADMIN@TEST.COM",
                        Email = "admin@test.com",
                        NormalizedUserName = "ADMIN@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "Admin1!")
                    },
                   
                    new ApplicationUser
                    {
                        Id = "3ce4f0e3-2476-4092-9209-4cce498cefa2", 
                        UserName = "user@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "USER@TEST.COM",
                        Email = "user@test.com",
                        NormalizedUserName = "USER@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "User1!")
                    }
                );

                context.UserRoles.AddRange(
                    new IdentityUserRole<string>
                    {
                        RoleId = "47991959-d03c-4c41-8938-83cf42af1067",
                        UserId = "3ce4f0e3-2476-4092-9209-4cce498cefa1"
                    },
                    new IdentityUserRole<string>
                    {
                        RoleId = "47991959-d03c-4c41-8938-83cf42af1068",
                        UserId = "3ce4f0e3-2476-4092-9209-4cce498cefa2"
                    }
                );

                context.SaveChanges();

            }
        }
    }
}
