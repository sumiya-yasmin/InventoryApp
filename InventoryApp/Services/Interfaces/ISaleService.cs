using InventoryApp.Models;
namespace InventoryApp.Services.Interfaces
{
    public interface ISaleService
    {
        Task<IEnumerable<Sale>> GetAllSaleAsync();
        Task<Sale?> GetSaleByIdAsync(int id);
        Task<Sale> AddSaleAsync(Sale sale);
        Task<Sale> UpdateSaleAsync(Sale sale);
        Task<Sale?> DeleteSaleAsync(int id);

    }
}