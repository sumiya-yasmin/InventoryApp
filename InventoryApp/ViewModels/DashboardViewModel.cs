using InventoryApp.Models;

namespace InventoryApp.ViewModels;

public class DashboardViewModel
{
    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<Customer> Customers { get; set; }
    public IEnumerable<Supplier> Suppliers { get; set; }
    public IEnumerable<Sale> Sales { get; set; }
    public IEnumerable<Purchase> Purchases { get; set; }
    

}