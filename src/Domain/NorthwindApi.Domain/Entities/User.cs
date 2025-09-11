using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindApi.Domain.Entities;

[Table("Users")]
[Index("Email", IsUnique = true)]
[Index("Username", IsUnique = true)]
public partial class User : BaseEntity<int>
{
    [Column("Username")]
    [StringLength(150)]
    public string Username { get; set; } = null!;

    [Column("Email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("HashedPassword")]
    [StringLength(255)]
    public string HashedPassword { get; set; } = null!;

    /// <summary>
    /// 1: Admin, 2: Staff, 3: Customer
    /// </summary>
    [Column("UserRoleID")]
    public int UserRoleId { get; set; }
    
    [ForeignKey(nameof(UserRoleId))]
    [InverseProperty(nameof(UserRole.Users))]
    public virtual UserRole? UserRole { get; set; }

    [InverseProperty(nameof(UserToken.User))]
    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
}
