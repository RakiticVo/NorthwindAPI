using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindApi.Domain.Entities;

[Table("UserRoles")]
public partial class UserRole : BaseEntity<int>
{
    [Column("UserRoleName")]
    [StringLength(50)]
    public string UserRoleName { get; set; } = null!;

    [Column("UserRoleDescription")]
    [StringLength(255)]
    public string? UserRoleDescription { get; set; }
    
    [InverseProperty(nameof(User.UserRole))]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
