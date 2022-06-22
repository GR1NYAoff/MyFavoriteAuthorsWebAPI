using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFavoriteAuthorsWebService.Configuration;
using MyFavoriteAuthorsWebService.Enum;
using MyFavoriteAuthorsWebService.Interfaces;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<Account> _accountRepository;
        private readonly ILogger<AccountService> _logger;
        private readonly IOptions<AuthOptions> _options;

        public AccountService(IBaseRepository<Account> accountRepository,
            ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<(StatusCode, string)> Login(Account user)
        {
            try
            {
                var account = await AuthenticateUser(user.Name, user.Password);

                if (user == null)
                    return (StatusCode.Unauthorized, string.Empty);

                var token = GenerateJWT(account);

                return (StatusCode.OK, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Register]: {ex.Message}");

                return (StatusCode.InternalServerError, string.Empty);
            }
        }

        public async Task<StatusCode> Register(Account user)
        {
            try
            {
                if (await AccountExists(user.Id, user.Name))
                    return StatusCode.BadRequest;

                _ = _accountRepository.Create(user);

                return StatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Register]: {ex.Message}");

                return StatusCode.InternalServerError;
            }
        }

        private Task<bool> AccountExists(Guid id, string name)
        {
            return _accountRepository.GetAll().AnyAsync(a => a.Id == id || a.Name == name);
        }

        private Task<Account?> AuthenticateUser(string name, string password)
        {
            return _accountRepository.GetAll().SingleOrDefaultAsync(a => a.Name == name && a.Password == password);
        }

        private string GenerateJWT(Account user)
        {
            var authParams = _options.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Sub, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
            };

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims: claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
