using System.ComponentModel.DataAnnotations;

namespace RestaurantNetwork.Api.DTO
{
    public class UpdateMenuDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
