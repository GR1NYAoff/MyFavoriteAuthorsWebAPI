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
        [MaxLength(20, ErrorMessage = "The name must contain no more than 20 characters")]
        [MinLength(3, ErrorMessage = "The name must contain at least 3 characters")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must contain at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string PasswordKey { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; }

        public Account()
        {

        }
    }
}
