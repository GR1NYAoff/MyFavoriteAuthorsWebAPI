using MyFavoriteAuthorsWebService.Data;
using MyFavoriteAuthorsWebService.Interfaces;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Repositories
{
    public class AccountRepository : IBaseRepository<Account>
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Account entity)
        {
            _ = await _dbContext.Accounts.AddAsync(entity);
            _ = await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Account entity)
        {
            _ = _dbContext.Accounts.Remove(entity);
            _ = await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Account> GetAll()
        {
            return _dbContext.Accounts;
        }

        public async Task<Account> Update(Account entity)
        {
            _ = _dbContext.Accounts.Update(entity);
            _ = await _dbContext.SaveChangesAsync();

            return entity;
        }

    }
}
