namespace P01_HospitalDatabase.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PatientMedicament
{
    [Required]
    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Medicament))]
    public int MedicamentId { get; set; }
    public Medicament Medicament { get; set; } = null!;
}
