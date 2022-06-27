using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFavoriteAuthorsWebService.Enum;
using MyFavoriteAuthorsWebService.Helpers;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Configuration
{
    public class AccountsConfiguration : IEntityTypeConfiguration<Account>
    {
        private const string ADMIN_PASSWORD = "AppAdminAccount123";
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            var hashed = HashPasswordHelper.HashPassword(ADMIN_PASSWORD);

            _ = builder.ToTable("Accounts");

            _ = builder.HasData(
                new Account
                {
                    Id = Guid.NewGuid(),
                    Name = "AppAdminAccount",
                    PasswordKey = hashed.Item2,
                    Password = hashed.Item1,
                    Role = Role.Admin
                });

            _ = builder.Property(x => x.Id).ValueGeneratedOnAdd();
            _ = builder.Property(x => x.Password).IsRequired();
            _ = builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

            _ = builder.HasKey(x => x.Id);
        }
    }
}
