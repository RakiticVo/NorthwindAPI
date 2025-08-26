using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NorthwindApi.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NorthwindContext>
{
    public NorthwindContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();
        optionsBuilder.UseSqlServer(connectionString!);

        return new NorthwindContext(optionsBuilder.Options);
    }
}
