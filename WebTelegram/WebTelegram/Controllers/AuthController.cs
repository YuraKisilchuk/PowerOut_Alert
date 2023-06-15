using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebTelegram.Abstract;
using WebTelegram.Data.Entities.Identity;
using WebTelegram.Models.Auth;

namespace WebTelegram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        public AuthController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user == null)
                    return BadRequest("Не правильно введена електронна пошта або пароль!");


                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                    return BadRequest("Не правильно введена електронна пошта або пароль!");

                var time = await _userManager.GetLockoutEndDateAsync(user);
                if (time != null)
                    return BadRequest($"Аккаунт заблоковано до {time.ToString()}");

                var token = await _jwtTokenService.CreateToken(user);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
