using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindApi.Api.Entities;

public partial class Shipper
{
    [Key]
    [Column("ShipperID")]
    public int ShipperId { get; set; }

    [StringLength(40)]
    public string CompanyName { get; set; } = null!;

    [StringLength(24)]
    public string? Phone { get; set; }

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

    [InverseProperty("ShipViaNavigation")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
