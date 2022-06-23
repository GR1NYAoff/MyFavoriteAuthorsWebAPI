using Microsoft.AspNetCore.Mvc;
using MyFavoriteAuthorsWebService.Interfaces;
using MyFavoriteAuthorsWebService.Models;

namespace MyFavoriteAuthorsWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var loginResult = await _accountService.Login(request);

            if (loginResult.Item1 != Enum.StatusCode.OK)
                return Unauthorized();

            return Ok(new { access_token = loginResult.Item2 });
        }

        [Route("reg")]
        [HttpPost]
        public async Task<IActionResult> RegistrationAsync([FromBody] LoginRequest request)
        {

            if (await _accountService.Register(request) != Enum.StatusCode.OK)
                return BadRequest();

            return Ok();

        }
    }
}
