using Microsoft.EntityFrameworkCore;
using MyFavoriteAuthorsWebService.Configuration;
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
        public DbSet<Bookmark> Bookmarks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Data for Accounts Table
            _ = modelBuilder.ApplyConfiguration(new AccountsConfiguration());
        }

    }
}
