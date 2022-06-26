using System.Net;
using System.Text.Json;
using MyFavoriteAuthorsWebService.Interfaces;
using MyFavoriteAuthorsWebService.JsonModels;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Services
{
    public class AuthorService : IAuthorService
    {
        private const string AuthorsUrl = "https://openlibrary.org/search/authors.json?q=";
        private const string AuthorBooksUrl = "https://openlibrary.org/authors/";
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(ILogger<AuthorService> logger)
        {
            _logger = logger;
        }

        public async Task<List<Doc>?> GetValidAuthors(string authorName)
        {
            try
            {
                var authorsJson = await GetAsync(AuthorsUrl + authorName);

                if (string.IsNullOrEmpty(authorsJson))
                    return null;

                var authors = JsonSerializer.Deserialize<RootAuthor>(authorsJson);

                if (authors == null ||
                    authors.NumFound <= 0 ||
                    authors.Docs == null ||
                    authors.Docs.All(d => d.WorkCount <= 0))
                {
                    return null;
                }

                var validAuthors = authors!.Docs
                    .Where(d => d.WorkCount >= 1)
                    .OrderByDescending(d => d.WorkCount)
                    .DistinctBy(d => d.Name)
                    .ToList();

                return validAuthors;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AuthorService.GetValidAuthors]: {ex.Message}");

                return null;
            }

        }

        public async Task<List<Book>?> GetBooks(string authorKey)
        {
            try
            {
                var booksJson = await GetAsync(AuthorBooksUrl + authorKey + "/works.json");

                if (string.IsNullOrEmpty(booksJson))
                    return null;

                var jsonObject = System.Text.Json.Nodes.JsonNode.Parse(booksJson);
                var entriesJsonArray = jsonObject?["entries"]?.AsArray();

                if (entriesJsonArray is null)
                    return null;

                var books = new List<Book>();

                foreach (var entry in entriesJsonArray)
                {
                    if (entry is null)
                        continue;

                    books.Add(new Book(entry?["title"]?.ToString(),
                                       entry?["description"]?.ToString()));
                }

                return books;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AuthorService.GetBooks]: {ex.Message}");

                return null;
            }

        }

        private async Task<string?> GetAsync(string uri)
        {
            if (WebRequest.Create(uri) is HttpWebRequest request)
            {
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using var response = (HttpWebResponse)await request.GetResponseAsync();
                using var stream = response.GetResponseStream();
                using var reader = new StreamReader(stream);

                return await reader.ReadToEndAsync();
            }

            return null;

        }

        public async Task<List<BookmarkRequest>?> GetValidBookmarksRequests(string authorName)
        {
            try
            {
                var booksRequest = new List<BookmarkRequest>();
                var validAuthors = await GetValidAuthors(authorName);

                if (validAuthors == null)
                    return booksRequest;

                foreach (var author in validAuthors)
                {
                    if (string.IsNullOrEmpty(author.Name) || string.IsNullOrEmpty(author.Key))
                        continue;

                    booksRequest.Add(new BookmarkRequest(author.Name!, author.Key!));
                }

                return booksRequest;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AuthorService.GetValidBookmarksRequests]: {ex.Message}");

                return new List<BookmarkRequest>();
            }
        }
    }
}
