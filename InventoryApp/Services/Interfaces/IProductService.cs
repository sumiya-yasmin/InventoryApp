using InventoryApp.Models;
namespace InventoryApp.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<Product?> DeleteProductAsync(int id);
        Task<int> GetProductStockAsync(int id);

    }
}