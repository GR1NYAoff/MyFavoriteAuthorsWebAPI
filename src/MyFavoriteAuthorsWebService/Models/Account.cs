using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyFavoriteAuthorsWebService.Enum;

namespace MyFavoriteAuthorsWebService.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public byte[]? PasswordKey { get; set; }

        [Required]
        public Role Role { get; set; }

        public Account() { }

        public Account(string name, string password, byte[] passwordKey)
        {
            Name = name;
            Password = password;
            PasswordKey = passwordKey;
            Role = Role.User;
        }
    }
}
