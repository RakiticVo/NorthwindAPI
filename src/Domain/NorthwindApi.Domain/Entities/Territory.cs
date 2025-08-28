using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindApi.Domain.Entities;

public partial class Territory : BaseEntity<string>
{
    [StringLength(50)]
    public string TerritoryDescription { get; set; } = null!;

    [Column("RegionID")]
    public int RegionId { get; set; }

    [ForeignKey("RegionId")]
    [InverseProperty("Territories")]
    public virtual Region Region { get; set; } = null!;

    [ForeignKey("Id")]
    [InverseProperty("Territories")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
