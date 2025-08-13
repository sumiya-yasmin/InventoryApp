using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Services.Implementations;

public class SupplierService : ISupplierService
{
    private readonly ApplicationDbContext _context;
    public SupplierService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Supplier>> GetAllSupplierAsync() => await _context.Suppliers.Include((s) => s.Purchases).ToListAsync();
    public async Task<Supplier?> GetSupplierByIdAsync(int id) => await _context.Suppliers.Include((s) => s.Purchases).FirstOrDefaultAsync((s) => s.SupplierId == id);
    public async Task<Supplier> AddSupplierAsync(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;

    }

    public async Task<Supplier> UpdateSupplierAsync(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task<Supplier?> DeleteSupplierAsync(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier != null)
        {
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }
        return supplier;
    }
}