namespace P01_StudentSystem.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

public class Resource
{
    [Key]
    public int ResourceId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    public string Url { get; set; } = null!;

    [Required]
    public ResourceType ResourceType { get; set; }

    [Required]
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
}
