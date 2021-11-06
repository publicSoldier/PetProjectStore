using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProjectStore.Api.Dtos;
using PetProjectStore.Api.Exceptions;
using PetProjectStore.Api.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PetProjectStore.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(Route)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        private const string Route = "account";

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("LogIn")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn ([FromBody] LogInDto logInDto)
        {
            try
            {
                var logInResult = await _accountService.LogInAsync(logInDto);
                
                await AuthenticateAsync(logInResult);

                return Ok("Вход успешен");
            }
            catch (LogInException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration([FromBody] RegistrationDto registrationDto)
        {
            try
            {
                await _accountService.RegistrationAsync(registrationDto);

                return Ok();
            }
            catch (RegistrationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        private async Task AuthenticateAsync(LogInResultDto logInResult)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, logInResult.Email),
                new Claim(ClaimTypes.NameIdentifier, logInResult.Id),
                new Claim(ClaimTypes.Role, logInResult.UserRole.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }
    }
}
