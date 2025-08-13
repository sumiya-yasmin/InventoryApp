using InventoryApp.Models;
namespace InventoryApp.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<IEnumerable<Purchase>> GetAllProductAsync();
        Task<Purchase?> GetProductByIdAsync(int id);
        Task<Purchase> AddProductAsync(Purchase purchase);
        Task<Purchase> UpdateProductAsync(Purchase purchase);
        Task<Purchase?> DeleteProductAsync(int id);

    }
}