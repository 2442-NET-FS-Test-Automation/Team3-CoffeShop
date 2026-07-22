using CoffeShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Data;

public class InventoryRepository : IInventoryRepository
{
    private readonly CoffeShopDbContext _context;

    public InventoryRepository(CoffeShopDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<InventoryItem>> GetAll()
    {

        return _context.InventoryItems.Include(i => i.product).ToList();
    }

    public async Task<InventoryItem?> GetInventoryItemBySku(string sku)
    {

        return _context.InventoryItems.Include(i => i.product).FirstOrDefault(i => i.product!.Sku == sku);
    }

    public async Task<InventoryItem> AddInventoryItem(string sku, string name, decimal price, int quantity)
    {

        InventoryItem newItem = new InventoryItem
        {
            product = new Product { Sku = sku, Name = name, Price = price },
            Stock = quantity
        };

        _context.InventoryItems.Add(newItem);
        _context.SaveChanges();

        return newItem;
    }

    public async Task<InventoryItem?> ChangeInventoryItem(string sku, string newName, decimal newPrice, int newQuantity)
    {


        InventoryItem? item = _context.InventoryItems
            .Include(i => i.product)
            .FirstOrDefault(i => i.product!.Sku == sku);

        if (item is null)
        {
            return null;
        }

        item.product!.Name = newName;
        item.product.Price = newPrice;
        item.Stock = newQuantity;

        _context.SaveChanges();

        return item;
    }

    public async Task<bool> RemoveBySku(string sku)
    {

        InventoryItem? itemToRemove = _context.InventoryItems.Include(i => i.product)
                                            .FirstOrDefault(i => i.product!.Sku == sku);

        if (itemToRemove is null)
        {
            return false;
        }

        _context.Products.Remove(itemToRemove.product!);

        _context.SaveChanges();
        return true;

    }
}