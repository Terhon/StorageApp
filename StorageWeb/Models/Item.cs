using System.ComponentModel.DataAnnotations;

namespace StorageWeb.Models;

public class Item
{
    public int Id { get; set; }
    public string? Name { get; set; }
    [DataType((DataType.Date))] 
    public DateTime AquisitionDate { get; set; }
    public decimal Price { get; set; }
}