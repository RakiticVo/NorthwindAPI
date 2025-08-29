using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Persistence;

public class NorthwindContext(DbContextOptions<NorthwindContext> options) : DbContext(options), IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

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
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserToken> UserTokens { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;

    public DbConnection Connection => Database.GetDbConnection();
    public DbTransaction? CurrentTransaction => _currentTransaction?.GetDbTransaction();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Khóa tổng hợp cho OrderDetail
        modelBuilder.Entity<OrderDetail>()
            .HasKey(od => new { od.OrderId, od.ProductId });

        // Tự tham chiếu cho Employee (ReportsTo)
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.ReportsToNavigation)
            .WithMany(m => m.InverseReportsToNavigation)
            .HasForeignKey(e => e.ReportsTo);
        
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Territories)
            .WithMany(t => t.Employees)
            .UsingEntity<Dictionary<string, object>>(
                "EmployeeTerritories",           // Tên bảng junction trong DB
                j => j
                    .HasOne<Territory>()
                    .WithMany()
                    .HasForeignKey("TerritoryID")    // Column name trong DB
                    .HasPrincipalKey(nameof(Territory.Id)),
                j => j  
                    .HasOne<Employee>()
                    .WithMany()
                    .HasForeignKey("EmployeeID")     // Column name trong DB
                    .HasPrincipalKey(nameof(Employee.Id)),
                j =>
                {
                    j.HasKey("EmployeeID", "TerritoryID");
                    j.ToTable("EmployeeTerritories"); // Tên bảng thực tế trong DB
                });
        
        // Ánh xạ khóa chính cho Category
        modelBuilder.Entity<Category>()
            .Property(c => c.Id)
            .HasColumnName("CategoryID");
        
        // Ánh xạ khóa chính cho Customer
        modelBuilder.Entity<Customer>()
            .Property(c => c.Id)
            .HasColumnName("CustomerID")
            .HasMaxLength(5);
        
        // Ánh xạ khóa chính cho CustomerDemographic
        modelBuilder.Entity<CustomerDemographic>()
            .Property(c => c.Id)
            .HasColumnName("CustomerTypeID")
            .HasMaxLength(5);
        
        // Ánh xạ khóa chính cho Employee
        modelBuilder.Entity<Employee>()
            .Property(e => e.Id)
            .HasColumnName("EmployeeID");
        
        // Ánh xạ khóa chính cho Order
        modelBuilder.Entity<Order>()
            .Property(e => e.Id)
            .HasColumnName("OrderID");
        
        // Ánh xạ khóa chính cho Product
        modelBuilder.Entity<Product>()
            .Property(e => e.Id)
            .HasColumnName("ProductID"); 
        
        // Ánh xạ khóa chính cho Region
        modelBuilder.Entity<Region>()
            .Property(e => e.Id)
            .HasColumnName("RegionID");
        
        // Ánh xạ khóa chính cho Shipper
        modelBuilder.Entity<Shipper>()
            .Property(e => e.Id)
            .HasColumnName("ShipperID");
        
        // Ánh xạ khóa chính cho Supplier
        modelBuilder.Entity<Supplier>()
            .Property(e => e.Id)
            .HasColumnName("SupplierID");
        
        // Ánh xạ khóa chính cho Territory
        modelBuilder.Entity<Territory>()
            .Property(e => e.Id)
            .HasColumnName("TerritoryID")
            .HasMaxLength(20);
        
        // Ánh xạ khóa chính cho User
        modelBuilder.Entity<User>()
            .Property(e => e.Id)
            .HasColumnName("user_id");
        
        // Ánh xạ khóa chính cho UserToken
        modelBuilder.Entity<UserToken>()
            .Property(e => e.Id)
            .HasColumnName("token_id");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                        entry.CurrentValues["CreatedAt"] = DateTime.UtcNow;
                    if (entry.Properties.Any(p => p.Metadata.Name == "CreatedBy"))
                        entry.CurrentValues["CreatedBy"] ??= "system";
                    break;
                case EntityState.Modified:
                    if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                        entry.CurrentValues["UpdatedAt"] = DateTime.UtcNow;
                    if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedBy"))
                        entry.CurrentValues["UpdatedBy"] ??= "system";
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
            throw new InvalidOperationException("A transaction is already in progress.");

        _currentTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("No transaction in progress.");

        try
        {
            await SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            return;

        try
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public override async ValueTask DisposeAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}