using System.ComponentModel.DataAnnotations.Schema;
using ServiceStack.DataAnnotations;

namespace MyFavoriteAuthorsWebService.Models
{
    public class Bookmark
    {
        [Required]
        [PrimaryKey, AutoIncrement]
        public int BookmarkId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string AuthorName { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string AuthorKey { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
