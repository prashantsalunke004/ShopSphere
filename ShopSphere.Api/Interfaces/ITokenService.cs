using ShopSphere.API.Models;
using System.Security.Claims;

namespace ShopSphere.API.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
