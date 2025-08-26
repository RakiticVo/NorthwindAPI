using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindApi.Api.Entities;

[Table("user_roles")]
public partial class UserRole
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

    [Column("row_version")]
    public byte[]? RowVersion { get; set; }
}
