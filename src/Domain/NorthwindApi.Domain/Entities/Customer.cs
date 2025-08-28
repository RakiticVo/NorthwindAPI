using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindApi.Domain.Entities;

[Index("City", Name = "City")]
[Index("CompanyName", Name = "CompanyName")]
[Index("PostalCode", Name = "PostalCode")]
[Index("Region", Name = "Region")]
public partial class Customer : BaseEntity<string>
{
    [StringLength(40)]
    public string CompanyName { get; set; } = null!;

    [StringLength(30)]
    public string? ContactName { get; set; }

    [StringLength(30)]
    public string? ContactTitle { get; set; }

    [StringLength(60)]
    public string? Address { get; set; }

    [StringLength(15)]
    public string? City { get; set; }

    [StringLength(15)]
    public string? Region { get; set; }

    [StringLength(10)]
    public string? PostalCode { get; set; }

    [StringLength(15)]
    public string? Country { get; set; }

    [StringLength(24)]
    public string? Phone { get; set; }

    [StringLength(24)]
    public string? Fax { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    [StringLength(50)]
    public string? CreatedBy { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    [Column("row_version")]
    public byte[]? RowVersion { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("Id")]
    [InverseProperty("Customers")]
    public virtual ICollection<CustomerDemographic> CustomerTypes { get; set; } = new List<CustomerDemographic>();
}
