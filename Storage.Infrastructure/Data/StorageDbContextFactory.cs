using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Storage.Infrastructure.Data;

public class StorageDbContextFactory: IDesignTimeDbContextFactory<StorageDbContext>
{
    public StorageDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StorageDbContext>();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // uses Web project's base path
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("StorageDbContext");
        optionsBuilder.UseSqlServer(connectionString);

        return new StorageDbContext(optionsBuilder.Options);
    }
}