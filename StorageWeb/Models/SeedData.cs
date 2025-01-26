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
                    Name = "When Harry Met Sally",
                    AcquisitionDate = DateTime.Parse("1989-2-12"),
                    Type = "a",
                    Price = 7.99M
                },
                new Item
                {
                    Name = "Ghostbusters ",
                    AcquisitionDate = DateTime.Parse("1984-3-13"),
                    Type = "b",
                    Price = 8.99M
                },
                new Item
                {
                    Name = "Ghostbusters 2",
                    AcquisitionDate = DateTime.Parse("1986-2-23"),
                    Type = "b",
                    Price = 9.99M
                },
                new Item
                {
                    Name = "Rio Bravo",
                    AcquisitionDate = DateTime.Parse("1959-4-15"),
                    Type = "c",
                    Price = 3.99M
                }
            );
            context.SaveChanges();
        }
    }
}
