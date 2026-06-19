using Microsoft.EntityFrameworkCore;
using RestaurantProject.DataModel;

namespace RestaurantNetwork.Api.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (await db.Restaurants.AnyAsync())
            {
                return;
            }

            var redRestaurant = new Restaurant
            {
                Color = "Red",
                Address = "Maribor, Slovenia",
                Rent = 1200,
                Menu = new Menu
                {
                    Name = "Main Menu",
                    Description = "Default menu for the red restaurant",
                    Dishes = new List<Dish>
                    {
                        new Dish
                        {
                            Name = "Classic Burger",
                            Price = 12.50m,
                            Category = "Main Course",
                            Description = "Beef burger with cheese, lettuce and house sauce"
                        },
                        new Dish
                        {
                            Name = "Caesar Salad",
                            Price = 8.90m,
                            Category = "Salad",
                            Description = "Fresh salad with chicken, parmesan and Caesar dressing"
                        },
                        new Dish
                        {
                            Name = "Chocolate Cake",
                            Price = 5.50m,
                            Category = "Dessert",
                            Description = "Homemade chocolate cake"
                        }
                    }
                }
            };

            var blueRestaurant = new Restaurant
            {
                Color = "Blue",
                Address = "Ljubljana, Slovenia",
                Rent = 1800,
                Menu = new Menu
                {
                    Name = "Lunch Menu",
                    Description = "Lunch menu with popular dishes",
                    Dishes = new List<Dish>
                    {
                        new Dish
                        {
                            Name = "Margherita Pizza",
                            Price = 10.00m,
                            Category = "Pizza",
                            Description = "Pizza with tomato sauce, mozzarella and basil"
                        },
                        new Dish
                        {
                            Name = "Pasta Carbonara",
                            Price = 11.50m,
                            Category = "Pasta",
                            Description = "Pasta with bacon, egg, parmesan and black pepper"
                        },
                        new Dish
                        {
                            Name = "Tomato Soup",
                            Price = 6.00m,
                            Category = "Soup",
                            Description = "Creamy tomato soup with herbs"
                        }
                    }
                }
            };

            var greenRestaurant = new Restaurant
            {
                Color = "Green",
                Address = "Celje, Slovenia",
                Rent = 950,
                Menu = new Menu
                {
                    Name = "Vegetarian Menu",
                    Description = "Vegetarian dishes and healthy options",
                    Dishes = new List<Dish>
                    {
                        new Dish
                        {
                            Name = "Vegetable Curry",
                            Price = 9.80m,
                            Category = "Main Course",
                            Description = "Vegetable curry with rice"
                        },
                        new Dish
                        {
                            Name = "Greek Salad",
                            Price = 7.50m,
                            Category = "Salad",
                            Description = "Salad with tomatoes, cucumber, olives and feta cheese"
                        },
                        new Dish
                        {
                            Name = "Fruit Bowl",
                            Price = 4.90m,
                            Category = "Dessert",
                            Description = "Fresh seasonal fruit bowl"
                        }
                    }
                }
            };

            db.Restaurants.AddRange(redRestaurant, blueRestaurant, greenRestaurant);

            await db.SaveChangesAsync();
        }
    }
}