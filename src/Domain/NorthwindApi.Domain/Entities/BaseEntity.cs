using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindApi.Domain.Entities;

public abstract class BaseEntity<TKey> : BaseAuditable
{
    [Key]
    [Column("Id")]
    public TKey Id { get; set; } = default!;
}