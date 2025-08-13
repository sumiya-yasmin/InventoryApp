using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Services.Implementations;

public class SaleService : ISaleService
{
    private readonly ApplicationDbContext _context;
    public SaleService(ApplicationDbContext context)
    {
        _context = context;
    }
public async Task<IEnumerable<Sale>> GetAllSaleAsync()
        {
            return await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.Customer)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<Sale?> GetSaleByIdAsync(int id)
        {
            return await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.SaleId == id);
        }

        public async Task<Sale> AddSaleAsync(Sale sale)
        {
            var product = await _context.Products.FindAsync(sale.ProductId);
            if (product != null)
            {
                if (product.QuantityInStock < sale.Quantity)
                    throw new InvalidOperationException("Not enough stock available.");
                
                product.QuantityInStock -= sale.Quantity;
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<Sale> UpdateSaleAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<Sale?> DeleteSaleAsync(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                var product = await _context.Products.FindAsync(sale.ProductId);
                if (product != null)
                    product.QuantityInStock += sale.Quantity;

                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
            return sale;
        }

}