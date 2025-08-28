using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindApi.Domain.Entities;

[Table("Region")]
public partial class Region : BaseEntity<int>
{
    [StringLength(50)]
    public string RegionDescription { get; set; } = null!;

    [InverseProperty("Region")]
    public virtual ICollection<Territory> Territories { get; set; } = new List<Territory>();
}
