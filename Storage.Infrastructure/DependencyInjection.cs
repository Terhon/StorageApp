using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Storage.Application.IngredientItem.Commands;
using Storage.Application.IngredientItem.Queries;
using Storage.Application.ItemType.Commands;
using Storage.Application.ItemType.Queries;
using Storage.Application.Recipe.Commands;
using Storage.Application.Recipe.Queries;
using Storage.Application.StorageItem.Commands;
using Storage.Application.StorageItem.Queries;
using Storage.Infrastructure.Services.IngredientItem;
using Storage.Infrastructure.Services.ItemType;
using Storage.Infrastructure.Services.Recipe;
using Storage.Infrastructure.Services.StorageItem;

namespace Storage.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IItemTypeCommandService, ItemTypeCommandService>();
        builder.Services.AddScoped<IItemTypeQueryService, ItemTypeQueryService>();
        
        builder.Services.AddScoped<IStorageItemCommandService, StorageItemCommandService>();
        builder.Services.AddScoped<IStorageItemQueryService, StorageItemQueryService>();
        
        builder.Services.AddScoped<IIngredientItemCommandService, IngredientItemCommandService>();
        builder.Services.AddScoped<IIngredientItemQueryService, IngredientItemQueryService>();
        
        builder.Services.AddScoped<IRecipeCommandService, RecipeCommandService>();
        builder.Services.AddScoped<IRecipeQueryService, RecipeQueryService>();
        
        return builder;
    }
}