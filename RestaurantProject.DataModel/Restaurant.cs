namespace RestaurantProject.DataModel
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Color { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public decimal Rent { get; set; }

        public Menu? Menu { get; set; }
    }
}