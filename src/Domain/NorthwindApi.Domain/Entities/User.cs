using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindApi.Domain.Entities;

[Table("users")]
[Index("Email", Name = "UQ__users__AB6E61649085BDA9", IsUnique = true)]
[Index("Username", Name = "UQ__users__F3DBC5723D782D5A", IsUnique = true)]
public partial class User : BaseEntity<int>
{
    [Column("username")]
    [StringLength(150)]
    public string Username { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("hashed_password")]
    [StringLength(255)]
    public string HashedPassword { get; set; } = null!;

    [Column("user_role_code")]
    [StringLength(50)]
    public string? UserRoleCode { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
}
