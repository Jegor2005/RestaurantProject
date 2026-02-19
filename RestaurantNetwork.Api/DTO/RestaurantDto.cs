namespace RestaurantNetwork.Api.DTO
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Color { get; set; } = "";
        public string Address { get; set; } = "";
        public decimal Rent { get; set; }
    }
}
