namespace ShopSphere.API.DTOs.Auth
{
    public class CurentUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
