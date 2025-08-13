using InventoryApp.Models;
namespace InventoryApp.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllProductAsync();
        Task<Customer?> GetProductByIdAsync(int id);
        Task<Customer> AddProductAsync(Customer customer);
        Task<Customer> UpdateProductAsync(Customer customer);
        Task<Customer?> DeleteProductAsync(int id);

    }
}