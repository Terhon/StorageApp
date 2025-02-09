using System.ComponentModel.DataAnnotations;

namespace StorageWeb.Models;

public class Recipe
{
    public int Id { get; set; }

    [StringLength(60)]
    [Required]
    public string? Name { get; set; }

    [StringLength(1000)]
    [Required]
    public string? Description { get; set; }

    public List<Item> Ingredients { get; } = [];
}