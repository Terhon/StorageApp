using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using StorageWeb.Data;

namespace StorageWeb.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new StorageWebContext(
                   serviceProvider.GetRequiredService<
                       DbContextOptions<StorageWebContext>>()))
        {
            // Look for any movies.
            if (context.Item.Any())
            {
                return;   // DB has been seeded
            }
            context.Item.AddRange(
                new Item
                {
                    Name = "Egg",
                    Unit = ""
                },
                new Item
                {
                    Name = "Milk",
                    Unit = "l"
                }
            );
            context.StorageItems.AddRange(
                new StorageItem
                {
                    Amount = 5,
                    AcquisitionDate = new DateTime(2024, 3, 16),
                    ItemId = 1
                },
                new StorageItem
                {
                    Amount = 2,
                    AcquisitionDate = new DateTime(2023, 6, 1),
                    ItemId = 2
                }
                );
            
            context.Recipe.AddRange(
                new Recipe
                {
                    Description = "example description",
                    Name = "example recipe"
                });
            
            context.IngredientItems.AddRange(
                new IngredientItem
                {
                    Amount = 7,
                    ItemId = 1,
                    RecipeId = 1
                },
                new IngredientItem
                {
                    Amount = 3,
                    ItemId = 2,
                    RecipeId = 1
                }
                );
            
            context.SaveChanges();
        }
    }
}
