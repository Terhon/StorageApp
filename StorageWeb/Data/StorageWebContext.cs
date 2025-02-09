using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorageWeb.Models;

namespace StorageWeb.Data
{
    public class StorageWebContext : DbContext
    {
        public StorageWebContext (DbContextOptions<StorageWebContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Recipe>()
                .HasMany<Item>(e => e.Ingredients)
                .WithMany();
        }

        public DbSet<StorageWeb.Models.Item> Item { get; set; } = default!;
        public DbSet<StorageWeb.Models.Recipe> Recipe { get; set; } = default!;
    }
}
