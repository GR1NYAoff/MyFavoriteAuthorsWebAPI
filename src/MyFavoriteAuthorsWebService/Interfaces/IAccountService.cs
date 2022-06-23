using MyFavoriteAuthorsWebService.Enum;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Interfaces
{
    public interface IAccountService
    {
        Task<StatusCode> Register(LoginRequest request);

        Task<(StatusCode, string)> Login(LoginRequest request);

    }
}
