using Microsoft.AspNetCore.Mvc.Rendering;

namespace StorageWeb.Models;

public class ItemTypeViewModel
{
    public List<Item>? Items { get; set; }
    public SelectList? Types { get; set; }
    public string? ItemType { get; set; }
    public string? SearchString { get; set; } 
}