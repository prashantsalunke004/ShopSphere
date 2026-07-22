using System.ComponentModel.DataAnnotations;
namespace ShopSphere.API.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
