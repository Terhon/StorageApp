using System.ComponentModel.DataAnnotations;

namespace StorageWeb.Models;

public class Item
{
    public int Id { get; set; }

    [StringLength(60)]
    [Required]
    public string Name { get; set; }

    [StringLength(20)]
    [Required]
    public string Unit { get; set; }
}