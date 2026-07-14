using Microsoft.EntityFrameworkCore;
using CoffeShop.Data.Entities;
using System.Dynamic;
using System.Data.Common;


namespace CoffeShop.Data;

public class CoffeShopDbContext : DbContext
{

    public CoffeShopDbContext(DbContextOptions<CoffeShopDbContext> options) : base(options) { }    
    
}