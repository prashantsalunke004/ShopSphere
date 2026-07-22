using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.API.DTOs.Auth;
using ShopSphere.API.Interfaces;

namespace ShopSphere.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        { 
         _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
             await _authService.RegisterAsync(registerDto);

            return Ok(new
            {
                Message="User Register Successfully."
            }
            );

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
        {


            var result = await _authService.RefreshTokenAsync(request);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();

            return NoContent();
        }

    }
}
