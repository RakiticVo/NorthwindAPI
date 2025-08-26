using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindApi.Api.Entities;

[Table("users")]
[Index("Email", Name = "UQ__users__AB6E61649085BDA9", IsUnique = true)]
[Index("Username", Name = "UQ__users__F3DBC5723D782D5A", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("username")]
    [StringLength(150)]
    public string Username { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("hashed_password")]
    [StringLength(255)]
    public string HashedPassword { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    [StringLength(150)]
    public string CreatedBy { get; set; } = null!;

    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [Column("updated_by")]
    [StringLength(150)]
    public string UpdatedBy { get; set; } = null!;

    [Column("user_role_code")]
    [StringLength(50)]
    public string? UserRoleCode { get; set; }

    [Column("row_version")]
    public byte[]? RowVersion { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
}
