using MyFavoriteAuthorsWebService.JsonModels;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Interfaces
{
    public interface IAuthorService
    {
        public Task<List<Doc>?> GetValidAuthors(string authorName);
        public Task<List<Book>?> GetBooks(string authorKey);
    }
}
