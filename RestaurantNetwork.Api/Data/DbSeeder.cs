using Microsoft.EntityFrameworkCore;
using RestaurantProject.DataModel;

namespace RestaurantNetwork.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Restaurants.AnyAsync())
            return;

        var items = new List<Restaurant>
        {
            new Restaurant { Color = "Red", Address = "Maribor", Rent = 1200 },
            new Restaurant { Color = "Blue", Address = "Ljubljana", Rent = 1500 },
            new Restaurant { Color = "Green", Address = "Graz", Rent = 1000 }
        };

        db.Restaurants.AddRange(items);
        await db.SaveChangesAsync();
    }
}