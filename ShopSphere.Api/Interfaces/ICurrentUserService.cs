using ShopSphere.API.DTOs.Auth;

namespace ShopSphere.API.Interfaces
{
    public interface ICurrentUserService
    {
        CurentUserDto GetCurrentUser();
    }


}
