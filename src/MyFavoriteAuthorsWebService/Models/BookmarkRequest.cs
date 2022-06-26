using System.ComponentModel.DataAnnotations;

namespace MyFavoriteAuthorsWebService.Models
{
    public class BookmarkRequest
    {
        [Required]
        public string AuthorName { get; set; } = string.Empty;
        [Required]
        public string AuthorKey { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public BookmarkRequest() { }

        public BookmarkRequest(string authorName, string authorKey)
        {
            AuthorName = authorName;
            AuthorKey = authorKey;
            Comment = string.Empty;
        }
    }
}
