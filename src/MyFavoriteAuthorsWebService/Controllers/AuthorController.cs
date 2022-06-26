using Microsoft.AspNetCore.Mvc;
using MyFavoriteAuthorsWebService.Interfaces;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("{authorName}")]
        public async Task<ActionResult<IEnumerable<BookmarkRequest>>> GetValidAuthorsList(string authorName)
        {
            return Ok(await _authorService.GetValidBookmarksRequests(authorName));
        }

    }
}
