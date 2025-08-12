using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models;

public class Category
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;


    [MaxLength(250)]
    public string? Description { get; set; }

    public ICollection<Product>? Products { get; set; } = new List<Product>();
}
