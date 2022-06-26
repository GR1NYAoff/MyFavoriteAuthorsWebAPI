using Microsoft.EntityFrameworkCore;
using MyFavoriteAuthorsWebService.Enum;
using MyFavoriteAuthorsWebService.Interfaces;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IBaseRepository<Bookmark> _bookmarkRepository;
        private readonly ILogger<BookmarkService> _logger;

        public BookmarkService(IBaseRepository<Bookmark> bookmarkRepository,
                               ILogger<BookmarkService> logger)
        {
            _bookmarkRepository = bookmarkRepository;
            _logger = logger;
        }

        public async Task<StatusCode> AddBookmark(Bookmark bookmark)
        {
            try
            {
                if (await BookmarkExists(bookmark.AuthorKey, bookmark.AccountId))
                    return StatusCode.BadRequest;

                await _bookmarkRepository.Create(bookmark);

                return StatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[BookmarkService.AddBookmark]: {ex.Message}");

                return StatusCode.InternalServerError;
            }

        }

        public async Task<IEnumerable<Bookmark>> GetAllBookmarks()
        {
            try
            {
                return await _bookmarkRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[BookmarkService.GetAllBookmarks]: {ex.Message}");

                return Enumerable.Empty<Bookmark>();
            }
        }

        public async Task<IEnumerable<Bookmark>> GetAvailableBookmarks(Guid userId)
        {
            try
            {
                return await _bookmarkRepository.GetAll().Where(b => b.Account.Id == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[BookmarkService.GetAvailableBookmarks]: {ex.Message}");

                return Enumerable.Empty<Bookmark>();
            }

        }

        public async Task<Bookmark?> GetBookmark(int bookmarkId, Guid userId)
        {
            try
            {
                return await _bookmarkRepository.GetAll()
                .FirstOrDefaultAsync(b => b.BookmarkId == bookmarkId && b.AccountId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[BookmarkService.GetBookmark]: {ex.Message}");

                return null;
            }
        }

        public async Task<StatusCode> RemoveBookmark(int bookmarkId, Guid userId)
        {
            try
            {
                var bookmark = await GetBookmark(bookmarkId, userId);

                if (bookmark == null)
                    return StatusCode.BadRequest;

                await _bookmarkRepository.Delete(bookmark);

                return StatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[BookmarkService.RemoveBookmark]: {ex.Message}");

                return StatusCode.InternalServerError;
            }
        }

        public async Task<StatusCode> UpdateBookmark(string comment, int bookmarkId, Guid userId)
        {
            try
            {
                var bookmark = await GetBookmark(bookmarkId, userId);

                if (bookmark == null)
                    return StatusCode.BadRequest;

                bookmark.Comment = comment;

                _ = await _bookmarkRepository.Update(bookmark);

                return StatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[BookmarkService.UpdateBookmark]: {ex.Message}");

                return StatusCode.InternalServerError;
            }
        }

        private async Task<bool> BookmarkExists(string authorKey, Guid userId)
        {
            return await _bookmarkRepository.GetAll()
            .AnyAsync(b => b.AuthorKey == authorKey && b.AccountId == userId);
        }

    }
}
