using MyFavoriteAuthorsWebService.Data;
using MyFavoriteAuthorsWebService.Interfaces;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Repositories
{
    public class BookmarkRepository : IBaseRepository<Bookmark>
    {
        private readonly ApplicationDbContext _dbContext;

        public BookmarkRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Bookmark entity)
        {
            _ = await _dbContext.Bookmarks.AddAsync(entity);
            _ = await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Bookmark entity)
        {
            _ = _dbContext.Bookmarks.Remove(entity);
            _ = await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Bookmark> GetAll()
        {
            return _dbContext.Bookmarks;
        }

        public async Task<Bookmark> Update(Bookmark entity)
        {
            _ = _dbContext.Bookmarks.Update(entity);
            _ = await _dbContext.SaveChangesAsync();

            return entity;
        }

    }
}
