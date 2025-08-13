using InventoryApp.Models;
namespace InventoryApp.Services.Interfaces
{
    public interface ISaleService
    {
        Task<IEnumerable<Sale>> GetAllProductAsync();
        Task<Sale?> GetProductByIdAsync(int id);
        Task<Sale> AddProductAsync(Sale sale);
        Task<Sale> UpdateProductAsync(Sale sale);
        Task<Sale?> DeleteProductAsync(int id);

    }
}