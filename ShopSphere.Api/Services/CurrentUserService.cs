using ShopSphere.API.DTOs.Auth;
using ShopSphere.API.Interfaces;
using System.Security.Claims;

namespace ShopSphere.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurentUserDto GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            var emailClaim = user.FindFirst(ClaimTypes.Email);

            var roleClaim = user.FindFirst(ClaimTypes.Role);

            if (idClaim == null || emailClaim == null || roleClaim == null)
                throw new UnauthorizedAccessException("Required claims are missing.");

            return new CurentUserDto
            {
                Id = int.Parse(idClaim.Value),
                Email = emailClaim.Value,
                Role = roleClaim.Value
            };
        }
    }
}
