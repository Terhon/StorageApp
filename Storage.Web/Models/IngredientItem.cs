using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace StorageWeb.Models;

public class IngredientItem
{
    public int Id { get; set; }
    
    public int RecipeId { get; set; }
    
    [ValidateNever]
    public Recipe Recipe { get; set; }
    
    public int ItemId { get; set; }

    [ValidateNever]
    public Item Item { get; set; }
    
    public double Amount { get; set; }
}