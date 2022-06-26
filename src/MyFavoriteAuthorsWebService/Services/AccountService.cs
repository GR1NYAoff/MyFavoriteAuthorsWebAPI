using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFavoriteAuthorsWebService.Configuration;
using MyFavoriteAuthorsWebService.Enum;
using MyFavoriteAuthorsWebService.Helpers;
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
            ILogger<AccountService> logger, IOptions<AuthOptions> options)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _options = options;
        }

        public async Task<(StatusCode, string)> Login(LoginRequest request)
        {
            try
            {
                if (!await AccountExists(request.Name))
                    return (StatusCode.Unauthorized, string.Empty);

                var account = await AuthenticateUser(request.Name, request.Password);

                if (account == null)
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

        public async Task<StatusCode> Register(LoginRequest request)
        {
            try
            {
                if (await AccountExists(request.Name))
                {
                    _logger.LogWarning($"[Register]: An account with this username already exists");

                    return StatusCode.BadRequest;
                }

                var hashed = HashPasswordHelper.HashPassword(request.Password);
                var newUser = new Account(request.Name, hashed.Item1, hashed.Item2);

                await _accountRepository.Create(newUser);

                return StatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Register]: {ex.Message}");

                return StatusCode.InternalServerError;
            }
        }

        private Task<bool> AccountExists(string name)
        {
            return _accountRepository.GetAll().AnyAsync(a => a.Name == name);
        }

        private async Task<Account?> AuthenticateUser(string name, string password)
        {
            var account = await _accountRepository.GetAll().FirstOrDefaultAsync(a => a.Name == name);
            var hashed = HashPasswordHelper.HashPassword(password, account!.PasswordKey);

            if (hashed.Item1 != account.Password)
                return null;

            return account;
        }

        private string GenerateJWT(Account user)
        {
            var authParams = _options.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Sub, user.Role.ToString())
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
