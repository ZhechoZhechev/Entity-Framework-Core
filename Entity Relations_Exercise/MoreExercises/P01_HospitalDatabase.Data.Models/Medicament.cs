namespace P01_HospitalDatabase.Data.Models;

using System.ComponentModel.DataAnnotations;

using Data.Common;

public class Medicament
{
    public Medicament()
    {
        this.Prescriptions = new HashSet<PatientMedicament>();
    }

    [Key]
    public int MedicamentId { get; set; }

    [Required]
    [StringLength(StringLengthConstants.MedicamentNameLength)]
    public string MedicamentName { get; set; } = null!;

    public ICollection<PatientMedicament> Prescriptions = null!;
}
