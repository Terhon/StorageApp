using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageWeb.Models;

public class Item
{
    public int Id { get; set; }
    
    public string? Name { get; set; }
    
    [Display(Name = "Acquisition Date")]
    [DataType(DataType.Date)] 
    public DateTime AcquisitionDate { get; set; }
    
    public string? Type { get; set; }
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public string? Rating { get; set; }
}
