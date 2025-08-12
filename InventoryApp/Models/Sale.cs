using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models;

public class Sale
{
    public int SaleId { get; set; }
    public DateTime SaleDate { get; set; } = DateTime.Now;

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePerUnit { get; set; }

    [Required]
    public int ProductId { get; set; }
    public required Product Product { get; set; }
    
    [Required]
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }


}