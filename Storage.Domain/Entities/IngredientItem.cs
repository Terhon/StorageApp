namespace Storage.Domain.Entities;

public class IngredientItem
{
    public int Id { get; set; }
    
    public Recipe Recipe { get; set; }
    
    public ItemType ItemType { get; set; }
    
    public double Amount { get; set; }
}