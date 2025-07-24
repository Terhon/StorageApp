namespace Storage.Domain.Entities;

public class IngredientItem
{
    public int Id { get; set; }
    
    public Recipe Recipe { get; set; }
    
    public ItemType Item { get; set; }
    
    public double Amount { get; set; }
}