namespace Eventmi.Infrastructure.Data.Models;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Събития
/// </summary>
[Comment("Събития")]
public class Event
{
    /// <summary>
    /// Идентификатор за запис
    /// </summary>
    [Key]
    [Comment("Идентификатор за запис")]
    public int Id { get; set; }

    /// <summary>
    /// Име на събитието
    /// </summary>
    [Required]
    [Comment("Име на събитието")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Начало на събитието
    /// </summary>
    [Required]
    [Comment("Начало на събитието")]
    public DateTime Start {get; set; }

    /// <summary>
    /// Край на събитието
    /// </summary>
    [Required]
    [Comment("Край на събитието")]
    public DateTime End { get; set; }

    /// <summary>
    /// Място на събитието
    /// </summary>
    [Required]
    [Comment("Място на събитието")]
    [StringLength(200)]
    public string Place { get; set; } = null!;

}

