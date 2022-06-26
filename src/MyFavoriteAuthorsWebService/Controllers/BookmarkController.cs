using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFavoriteAuthorsWebService.Interfaces;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;
        private readonly IAuthorService _authorService;
        private string UserId => User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        public BookmarkController(IBookmarkService bookmarkService,
                                  IAuthorService authorService)
        {
            _bookmarkService = bookmarkService;
            _authorService = authorService;
        }

        // GET: api/<BookmarkController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bookmark>>> GetAvailableBookmarks()
        {
            if (!Guid.TryParse(UserId, out var userId))
                return Unauthorized();

            return Ok(await _bookmarkService.GetAvailableBookmarks(userId));

        }

        // GET api/<BookmarkController>/1
        [HttpGet("{bookmarkId}")]
        public async Task<ActionResult<Bookmark>> GetBookmark(int bookmarkId)
        {
            if (!Guid.TryParse(UserId, out var userId))
                return Unauthorized();

            var bookmark = await _bookmarkService.GetBookmark(bookmarkId, userId);

            return bookmark == null ? NotFound() : Ok(bookmark);
        }

        // GET api/<BookmarkController>/1
        [HttpGet("{bookmarkId}/books")]
        public async Task<ActionResult<Book>> GetBookmarkBooks(int bookmarkId)
        {
            if (!Guid.TryParse(UserId, out var userId))
                return Unauthorized();

            var bookmark = await _bookmarkService.GetBookmark(bookmarkId, userId);
            var books = await _authorService.GetBooks(bookmark!.AuthorKey);

            return books == null ? NotFound() : Ok(books);
        }

        // POST api/<BookmarkController>
        [HttpPost]
        public async Task<ActionResult> PostBookmark([FromBody] BookmarkRequest request)
        {
            if (!Guid.TryParse(UserId, out var userId))
                return Unauthorized();

            var newBookmark = new Bookmark(request.AuthorName, request.AuthorKey,
                                           request.Comment, userId);

            if (await _bookmarkService.AddBookmark(newBookmark) != Enum.StatusCode.OK)
                return BadRequest();

            return Ok();
        }

        // PUT api/<BookmarkController>/2
        [HttpPut("{bookmarkId}")]
        public async Task<IActionResult> PutBookmark(int bookmarkId, [FromBody] string comment)
        {
            if (!Guid.TryParse(UserId, out var userId))
                return Unauthorized();

            if (await _bookmarkService.UpdateBookmark(comment, bookmarkId, userId) != Enum.StatusCode.OK)
                return BadRequest();

            return Ok();
        }

        // DELETE api/<BookmarkController>/3
        [HttpDelete("{bookmarkId}")]
        public async Task<IActionResult> DeleteBookmark(int bookmarkId)
        {
            if (!Guid.TryParse(UserId, out var userId))
                return Unauthorized();

            if (await _bookmarkService.RemoveBookmark(bookmarkId, userId) != Enum.StatusCode.OK)
                return BadRequest();

            return Ok();
        }
    }
}
