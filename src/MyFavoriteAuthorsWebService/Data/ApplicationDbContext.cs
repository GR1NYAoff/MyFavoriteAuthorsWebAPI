using Microsoft.EntityFrameworkCore;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _ = Database.EnsureCreated();
        }

        public DbSet<Account> Accounts { get; set; }

    }
}
