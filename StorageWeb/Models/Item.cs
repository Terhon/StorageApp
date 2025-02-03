using System.ComponentModel.DataAnnotations;

namespace StorageWeb.Models;

public class Item
{
    public int Id { get; set; }

    [StringLength(60)]
    [Required]
    public string? Name { get; set; }

    [Display(Name = "Acquisition Date")]
    [DataType(DataType.Date)]
    public DateTime AcquisitionDate { get; set; }
    
    public int Amount { get; set; }
}