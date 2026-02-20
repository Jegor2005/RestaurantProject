using System.ComponentModel.DataAnnotations;

namespace RestaurantNetwork.Api.DTO
{
    public class UpdateRestaurantDto
    {
        [Required]
        [MaxLength(50)]
        public string Color { get; set; } = "";
        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = "";

        [Range(0, 1_000_000)]
        public decimal Rent { get; set; }
    }
}
