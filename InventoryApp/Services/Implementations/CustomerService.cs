using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Services.Implementations;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;
    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Customer>> GetAllCustomerAsync() => await _context.Customers.Include((c) => c.Sales).ToListAsync();
    public async Task<Customer?> GetCustomerByIdAsync(int id) => await _context.Customers.Include((c) => c.Sales).FirstOrDefaultAsync((c) => c.CustomerId == id);
    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;

    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer?> DeleteCustomerAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
        return customer;
    }
}