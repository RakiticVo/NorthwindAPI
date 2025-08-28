using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindApi.Domain.Entities;

public partial class Shipper : BaseEntity<int>
{
    [StringLength(40)]
    public string CompanyName { get; set; } = null!;

    [StringLength(24)]
    public string? Phone { get; set; }

    [InverseProperty("ShipViaNavigation")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
