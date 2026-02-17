using Microsoft.EntityFrameworkCore;
using RestaurantProject.DataModel;

namespace RestaurantNetwork.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
}