namespace RestaurantNetwork.Api.DTO
{
    public class MenuDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int RestaurantId { get; set; }
    }
}