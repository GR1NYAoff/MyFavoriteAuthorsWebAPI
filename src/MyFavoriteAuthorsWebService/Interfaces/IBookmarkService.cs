using MyFavoriteAuthorsWebService.Enum;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Interfaces
{
    public interface IBookmarkService
    {
        Task<Bookmark?> GetBookmark(int bookmarkId, Guid userId);
        Task<IEnumerable<Bookmark>> GetAvailableBookmarks(Guid userId);
        Task<IEnumerable<Bookmark>> GetAllBookmarks();
        Task<StatusCode> AddBookmark(Bookmark bookmark);
        Task<StatusCode> UpdateBookmark(string comment, int bookmarkId, Guid userId);
        Task<StatusCode> RemoveBookmark(int bookmarkId, Guid userId);

    }
}
