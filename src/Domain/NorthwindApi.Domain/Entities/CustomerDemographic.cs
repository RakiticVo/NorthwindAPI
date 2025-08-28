using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindApi.Domain.Entities;

public partial class CustomerDemographic : BaseEntity<string>
{
    [Column(TypeName = "ntext")]
    public string? CustomerDesc { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("CustomerTypes")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
