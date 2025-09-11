using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindApi.Domain.Entities;

[Table("UserTokens")]
[Index("UserId", "DeviceType", Name = "uq_user_device", IsUnique = true)]
public partial class UserToken : BaseEntity<int>
{
    [Column("UserID")]
    public int UserId { get; set; }

    [Column("AccessToken")]
    [StringLength(1024)]
    public string AccessToken { get; set; } = null!;

    [Column("TokenType")]
    [StringLength(50)]
    public string TokenType { get; set; } = null!;

    [Column("DeviceType")]
    [StringLength(50)]
    public string DeviceType { get; set; } = null!;

    [Column("RefreshToken")]
    [StringLength(1024)]
    public string RefreshToken { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserTokens")]
    public virtual User User { get; set; } = null!;
}
