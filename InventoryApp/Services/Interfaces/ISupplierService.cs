using InventoryApp.Models;
namespace InventoryApp.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllProductAsync();
        Task<Supplier?> GetProductByIdAsync(int id);
        Task<Supplier> AddProductAsync(Supplier supplier);
        Task<Supplier> UpdateProductAsync(Supplier supplier);
        Task<Supplier?> DeleteProductAsync(int id);

    }
}