namespace P01_HospitalDatabase.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Common;


public class Diagnose
{
    [Key]
    public int DiagnoseId { get; set; }

    [Required]
    [StringLength(StringLengthConstants.DiagnoseNameLength)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(StringLengthConstants.DiagnoseCommentsLength)]
    public string Comments { get; set; } = null!;

    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
}
