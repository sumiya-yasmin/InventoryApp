using InventoryApp.Models;
namespace InventoryApp.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<IEnumerable<Purchase>> GetAllPurchaseAsync();
        Task<Purchase?> GetPurchaseByIdAsync(int id);
        Task<Purchase> AddPurchaseAsync(Purchase purchase);
        Task<Purchase> UpdatePurchaseAsync(Purchase purchase);
        Task<Purchase?> DeletePurchaseAsync(int id);

    }
}