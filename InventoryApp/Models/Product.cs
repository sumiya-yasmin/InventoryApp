using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models;

public class Product
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PurchasePrice { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SellPrice { get; set; }

    [Required]
    public int QuantityInStock { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();

}