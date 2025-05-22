using Microsoft.EntityFrameworkCore;
using StorageWeb.Models;

namespace StorageWeb.Data
{
    public class StorageWebContext(DbContextOptions<StorageWebContext> options) : DbContext(options)
    {
        public DbSet<Item> Item { get; set; } = default!;
        public DbSet<Recipe> Recipe { get; set; } = default!;
        public DbSet<IngredientItem> IngredientItems  { get; set; } = default!;
        public DbSet<StorageItem> StorageItems { get; set; } = default!;
        
    }
}
