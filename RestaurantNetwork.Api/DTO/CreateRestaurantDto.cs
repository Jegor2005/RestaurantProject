namespace RestaurantNetwork.Api.DTO
{
    public class CreateRestaurantDto
    {
        public string Color { get; set; } = "";
        public string Address { get; set; } = "";
        public decimal Rent { get; set; }
    }
}
