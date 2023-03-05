namespace P01_HospitalDatabase.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Common;

public class Visitation
{
    [Key]
    public int VisitationId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [StringLength(StringLengthConstants.VisitationCommentLength)]
    public string Comments { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
}
