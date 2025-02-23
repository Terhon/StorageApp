namespace StorageWeb.Models;

public class IngredientItem
{
    public int Id { get; set; }
    
    public int RecipeId { get; set; }
    
    public Recipe Recipe { get; set; }
    
    public int ItemId { get; set; }

    public Item Item { get; set; }
    
    public double Amount { get; set; }
}