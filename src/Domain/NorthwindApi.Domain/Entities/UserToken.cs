using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindApi.Api.Entities;

[Table("user_tokens")]
[Index("UserId", "DeviceType", Name = "uq_user_device", IsUnique = true)]
public partial class UserToken
{
    [Key]
    [Column("token_id")]
    public int TokenId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("access_token")]
    [StringLength(1024)]
    public string AccessToken { get; set; } = null!;

    [Column("token_type")]
    [StringLength(50)]
    public string TokenType { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("device_type")]
    [StringLength(50)]
    public string DeviceType { get; set; } = null!;

    [Column("refresh_token")]
    [StringLength(1024)]
    public string RefreshToken { get; set; } = null!;

    [Column("created_by")]
    [StringLength(150)]
    public string CreatedBy { get; set; } = null!;

    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [Column("updated_by")]
    [StringLength(150)]
    public string UpdatedBy { get; set; } = null!;

    [Column("row_version")]
    public byte[]? RowVersion { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserTokens")]
    public virtual User User { get; set; } = null!;
}
