using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Services.Implementations;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Product>> GetAllProductAsync() => await _context.Products.Include((p) => p.Category).ToListAsync();
    public async Task<Product?> GetProductByIdAsync(int id) => await _context.Products.Include((p) => p.Category).FirstOrDefaultAsync((p) => p.ProductId == id);
    public async Task<Product> AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;

    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        return product;
    }

     public async Task<int> GetProductStockAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Purchases)
                .Include(p => p.Sales)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return 0;

            var totalPurchased = product.Purchases.Sum(p => p.Quantity);
            var totalSold = product.Sales.Sum(s => s.Quantity);

            return totalPurchased - totalSold;
        }
}