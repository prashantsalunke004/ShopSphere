using ShopSphere.API.DTOs.Auth;

namespace ShopSphere.API.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto registerDto);

        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);

        Task LogoutAsync();
    }
}
