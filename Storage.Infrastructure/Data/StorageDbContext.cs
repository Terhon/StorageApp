using Microsoft.EntityFrameworkCore;
using Storage.Domain.Entities;

namespace Storage.Infrastructure.Data;

public class StorageDbContext(DbContextOptions<StorageDbContext> options) : DbContext(options)
{
    public DbSet<ItemType> ItemTypes => Set<ItemType>();
    public DbSet<StorageItem> StorageItems => Set<StorageItem>();
    public DbSet<IngredientItem> IngredientItems => Set<IngredientItem>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ItemType>().HasKey(x => x.Id);
        
        modelBuilder.Entity<StorageItem>().HasKey(x => x.Id);
        modelBuilder.Entity<StorageItem>()
            .HasOne(x => x.ItemType)
            .WithMany()
            .HasForeignKey("ItemTypeId")
            .IsRequired();
        
        modelBuilder.Entity<IngredientItem>().HasKey(x => x.Id);
        modelBuilder.Entity<IngredientItem>()
            .HasOne(x => x.ItemType)
            .WithMany()
            .HasForeignKey("ItemTypeId")
            .IsRequired();
        modelBuilder.Entity<IngredientItem>()
            .HasOne(x => x.Recipe)
            .WithMany()
            .HasForeignKey("RecipeId")
            .IsRequired();
                
        modelBuilder.Entity<Recipe>().HasKey(x => x.Id);
    }
}