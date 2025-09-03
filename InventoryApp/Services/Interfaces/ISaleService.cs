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
        Task<IEnumerable<Sale>> GetRecentSalesAsync(int count);
        Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime start, DateTime end);
        Task<decimal> GetTotalSalesAmountAsync(DateTime start, DateTime end);

    }
}