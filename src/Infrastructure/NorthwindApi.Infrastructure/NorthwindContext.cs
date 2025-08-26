using Microsoft.EntityFrameworkCore;
using NorthwindApi.Api.Entities;

namespace NorthwindApi.Infrastructure
{
    public class NorthwindContext(DbContextOptions<NorthwindContext> options) : DbContext(options)
    {
        // DbSets
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<CustomerDemographic> CustomerDemographics { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<Shipper> Shippers { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Territory> Territories { get; set; } = null!;
        public DbSet<Region> Regions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Key cho OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId });
            

            // Self-referencing cho Employee (ReportsTo)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.ReportsToNavigation)
                .WithMany(m => m.InverseReportsToNavigation)
                .HasForeignKey(e => e.ReportsTo);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    {
                        if (entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                            entry.CurrentValues["CreatedAt"] = DateTime.UtcNow;

                        if (entry.Properties.Any(p => p.Metadata.Name == "CreatedBy"))
                            entry.CurrentValues["CreatedBy"] ??= "system";
                        break;
                    }
                    case EntityState.Modified:
                    {
                        if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                            entry.CurrentValues["UpdatedAt"] = DateTime.UtcNow;

                        if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedBy"))
                            entry.CurrentValues["UpdatedBy"] ??= "system";
                        break;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
