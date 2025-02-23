using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageWeb.Models;

public class StorageItem
{
    public int Id { get; set; }
    
    public int ItemId { get; set; }

    public Item Item { get; set; }
    
    [Display(Name = "Acquisition Date")]
    [DataType(DataType.Date)]
    public DateTime AcquisitionDate { get; set; }
    
    public int Amount { get; set; }
    
    [NotMapped]
    public string AmountWithUnit => $"{Amount} {Item.Unit}";
}