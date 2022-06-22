using System.ComponentModel.DataAnnotations;

namespace MyFavoriteAuthorsWebService.Models
{
    public class LoginRequest
    {
        [Required]
        [MaxLength(20, ErrorMessage = "The name must contain no more than 20 characters")]
        [MinLength(3, ErrorMessage = "The name must contain at least 3 characters")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must contain at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }
}
