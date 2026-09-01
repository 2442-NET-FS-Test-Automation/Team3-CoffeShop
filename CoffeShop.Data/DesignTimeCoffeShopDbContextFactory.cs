using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoffeShop.Data;

public class DesignTimeCoffeShopDbContextFactory
    : IDesignTimeDbContextFactory<CoffeShopDbContext>
{
    public CoffeShopDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__CoffeShop")
            ?? throw new InvalidOperationException(
                "ConnectionStrings__CoffeShop is required for EF migrations.");

        var options = new DbContextOptionsBuilder<CoffeShopDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new CoffeShopDbContext(options);
    }
}
