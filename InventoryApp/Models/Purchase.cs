using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace InventoryApp.Models;

public class Purchase
{
    public int PurchaseId { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.Now;

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePerUnit { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

}