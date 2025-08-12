using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models;

public class Customer
{
    public int CustomerId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(15)]
    public string? Phone { get; set; }
    
    [StringLength(100)]
    public string? Email { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}