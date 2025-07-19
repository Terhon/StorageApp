using Microsoft.EntityFrameworkCore;
using Storage.Domain.Entities;

namespace Storage.Infrastructure.Data;

public class StorageDbContext(DbContextOptions<StorageDbContext> options) : DbContext(options)
{
    public DbSet<ItemType> ItemTypes => Set<ItemType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ItemType>().HasKey(x => x.Id);
    }
}