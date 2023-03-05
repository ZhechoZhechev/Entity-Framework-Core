namespace P01_HospitalDatabase.Data.Models;

using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using P01_HospitalDatabase.Data.Common;

public class Patient
{
    public Patient()
    {
        this.Visitations = new HashSet<Visitation>();
        this.Diagnoses = new HashSet<Diagnose>();
        this.Prescriptions = new HashSet<PatientMedicament>();
    }

    [Key]
    public int PatientId { get; set; }

    [Required]
    [StringLength(StringLengthConstants.PatientsNameLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(StringLengthConstants.PatientsNameLength)]
    public string LastName { get; set; } = null!;

    [Required]
    [StringLength(StringLengthConstants.PatientAddressLength)]
    public string Address { get; set; } = null!;

    [Required]
    [StringLength(StringLengthConstants.PatientEmailLength)]
    public string Email { get; set; } = null!;

    [Required]
    public bool HasInsurance { get; set; }

    public ICollection<Visitation> Visitations { get; set; } = null!;

    public ICollection<Diagnose> Diagnoses { get; set; } = null!;

    public ICollection<PatientMedicament> Prescriptions = null!;
}
