using Microsoft.EntityFrameworkCore;
using CoffeShop.Data.Entities;
using System.Dynamic;
using System.Data.Common;
using System.IO.Compression;


namespace CoffeShop.Data;

public class CoffeShopDbContext : DbContext
{

    public CoffeShopDbContext(DbContextOptions<CoffeShopDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        //Entities
        b.Entity<User>(e =>
        {

            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Email).IsUnique();
            e.Property(p => p.Email).HasMaxLength(100);
            e.HasIndex(p => p.Username).IsUnique();
            e.Property(p => p.Username).HasMaxLength(100);
            e.Property(p => p.Password).HasMaxLength(100);
            e.Property(p => p.Role).HasMaxLength(100);

        });

        b.Entity<Order>(e =>
        {

            e.HasKey(p => p.Id);
            e.HasOne(p => p.User)
                 .WithOne(u => u.Order)
                 .HasForeignKey<User>(i => i.Id);
        });

        b.Entity<Product>(e =>
        {

            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Sku).IsUnique();
            e.Property(p => p.Name).HasMaxLength(100);
            e.Property(p => p.Price).HasDefaultValue(0.00);
            e.Property(p => p.Price).HasColumnType("decimal(10,2)");
        });

        b.Entity<OrderLine>(e =>
        {

            e.HasKey(o => o.Id);
            e.HasOne(o => o.Order)
                .WithMany(u => u.orderLines)
                .HasForeignKey(i => i.OrderId);
        });

        b.Entity<InventoryItem>(e =>
        {
            e.HasKey(o => o.Id);
            e.HasOne(o => o.product)
                 .WithMany(u => u.inventoryItems)
                 .HasForeignKey(i => i.ProductId);
        });

        //Seeding some items:
        //Drinks
        b.Entity<Product>().HasData(
            //Hot drinks
            new Product { Id = 1, Sku = "HOT-AME-01", Name = "American", Price = 50.00m },
            new Product { Id = 2, Sku = "HOT-LAT-02", Name = "Latte", Price = 65.00m },
            new Product { Id = 3, Sku = "HOT-CAP-03", Name = "Capuccino", Price = 60.00m },
            new Product { Id = 4, Sku = "HOT-TAR-04", Name = "Taro", Price = 80.00m },
            new Product { Id = 5, Sku = "HOT-CHA-05", Name = "Natural Chai", Price = 90.00m },
            //Cold drinks
            new Product { Id = 6, Sku = "COL-LAT-06", Name = "Iced Latte", Price = 70.00m },
            new Product { Id = 7, Sku = "COL-AME-07", Name = "Iced American", Price = 60.00m },
            new Product { Id = 8, Sku = "COL-TAR-08", Name = "Iced Taro", Price = 85.00m },
            new Product { Id = 9, Sku = "COL-CHA-09", Name = "Iced Chai", Price = 95.00m }
        );

        b.Entity<InventoryItem>().HasData(
            new InventoryItem { Id = 1, ProductId = 1, Stock = 5 },
            new InventoryItem { Id = 2, ProductId = 2, Stock = 8 },
            new InventoryItem { Id = 3, ProductId = 3, Stock = 4 },
            new InventoryItem { Id = 4, ProductId = 4, Stock = 6 },
            new InventoryItem { Id = 5, ProductId = 5, Stock = 2 },
            new InventoryItem { Id = 6, ProductId = 6, Stock = 7 },
            new InventoryItem { Id = 7, ProductId = 7, Stock = 5 },
            new InventoryItem { Id = 8, ProductId = 8, Stock = 10 },
            new InventoryItem { Id = 9, ProductId = 9, Stock = 3 }
        );

    }

}