using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models;

public class Supplier
{
    public int SupplierId { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string Address { get; set; } = string.Empty;


    [MaxLength(50)]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

}
