using MyFavoriteAuthorsWebService.Enum;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Interfaces
{
    public interface IAccountService
    {
        Task<StatusCode> Register(Account user);

        Task<(StatusCode, string)> Login(Account user);

    }
}
