using Microsoft.EntityFrameworkCore;
using RestaurantProject.DataModel;

namespace RestaurantNetwork.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<Dish> Dishes => Set<Dish>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>()
            .ToTable("Restaurants");

        modelBuilder.Entity<Menu>()
            .ToTable("Menus");

        modelBuilder.Entity<Dish>()
            .ToTable("Dishes");

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.Rent)
            .HasConversion<double>()
            .HasColumnType("REAL");

        modelBuilder.Entity<Dish>()
            .Property(d => d.Price)
            .HasConversion<double>()
            .HasColumnType("REAL");

        modelBuilder.Entity<Restaurant>()
            .HasOne(r => r.Menu)
            .WithOne(m => m.Restaurant)
            .HasForeignKey<Menu>(m => m.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Menu>()
            .HasMany(m => m.Dishes)
            .WithOne(d => d.Menu)
            .HasForeignKey(d => d.MenuId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}