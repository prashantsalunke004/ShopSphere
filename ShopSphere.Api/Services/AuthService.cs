using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopSphere.API.Data;
using ShopSphere.API.DTOs.Auth;
using ShopSphere.API.Enums;
using ShopSphere.API.Exceptions;
using ShopSphere.API.Interfaces;
using ShopSphere.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopSphere.API.Services
{
    public class AuthService :IAuthService
    {
        private readonly AppDbContext _context ;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUserService;

        public AuthService(AppDbContext context, IConfiguration configuration, ITokenService tokenService, ICurrentUserService currentUserService)
        {
            _context = context;
            _configuration = configuration;
            _tokenService = tokenService;
            _currentUserService = currentUserService;
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {

            bool ismailexist = await _context.Users.AnyAsync(u=>u.Email==registerDto.Email);
            if (ismailexist) {

                throw new Exception("Email Already Exist.");
            }
            var user = new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = Role.Customer

            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!isPasswordCorrect)
            {
                throw new Exception("Invalid email or password");
            }

            // Generate Access Token
            var accessToken = _tokenService.GenerateAccessToken(user);

            // Generate Refresh Token
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
                Convert.ToInt32(_configuration["Jwt:RefreshTokenExpiryInDays"]));

            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,

                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(
        Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),

                RefreshToken = user.RefreshToken,

                RefreshTokenExpiry = user.RefreshTokenExpiryTime.Value
            };

        }

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);

            if (principal == null)
            {
                throw new UnauthorizedException("Invalid Access Token");
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(userId));

            if (user == null)
            {
                throw new UnauthorizedException("User Not Found");
            }

            if (user.RefreshToken != request.RefreshToken)
            {
                throw new UnauthorizedException("Invalid Refresh Token");
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Refresh Token Expired");
            }

            // Generate New Access Token
            var newAccessToken = _tokenService.GenerateAccessToken(user);

            // Generate New Refresh Token
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // Update User
            user.RefreshToken = newRefreshToken;

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
                Convert.ToInt32(_configuration["Jwt:RefreshTokenExpiryInDays"]));

            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,

                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),

                RefreshToken = newRefreshToken,

                RefreshTokenExpiry = user.RefreshTokenExpiryTime.Value
            };
        }


        public async Task LogoutAsync()
        {
            var currentUser = _currentUserService.GetCurrentUser();

            var user = await _context.Users.FindAsync(currentUser.Id);

            if (user == null)
            {
                throw new NotFoundException("User Not Found");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _context.SaveChangesAsync();
        }



    }
}
