using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindApi.Domain.Entities;

public abstract class BaseAuditable
{
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_by")]
    [StringLength(50)]
    public string? CreatedBy { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    [Timestamp]
    [Column("row_version")]
    public byte[] RowVersion { get; set; } = null!;
}