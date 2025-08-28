using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindApi.Domain.Entities;

[Table("user_roles")]
public partial class UserRole : BaseAuditable
{
    [Key]
    [Column("user_role_code")]
    [StringLength(50)]
    public string UserRoleCode { get; set; } = null!;

    [Column("user_role_name")]
    [StringLength(50)]
    public string UserRoleName { get; set; } = null!;

    [Column("user_role_description")]
    [StringLength(255)]
    public string? UserRoleDescription { get; set; }
}
