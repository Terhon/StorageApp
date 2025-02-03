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
                    Name = "Eggs",
                    AcquisitionDate = DateTime.Parse("1989-2-12"),
                    Amount = 5
                },
                new Item
                {
                    Name = "Milk cartons",
                    AcquisitionDate = DateTime.Parse("1984-3-13"),
                    Amount = 10,
                },
                new Item
                {
                    Name = "Banana",
                    AcquisitionDate = DateTime.Parse("1986-2-23"),
                    Amount = 3,
                },
                new Item
                {
                    Name = "Apple",
                    AcquisitionDate = DateTime.Parse("1959-4-15"),
                    Amount = 1,
                }
            );
            context.SaveChanges();
        }
    }
}
