namespace MyFavoriteAuthorsWebService.Models
{
    public class Book
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Book() { }
        public Book(string? title, string? description)
        {
            Title = title ?? "Unknown";
            Description = description ?? string.Empty;
        }
    }
}
