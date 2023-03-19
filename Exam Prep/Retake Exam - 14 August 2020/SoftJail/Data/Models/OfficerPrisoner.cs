namespace SoftJail.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OfficerPrisoner
    {
        [ForeignKey(nameof(Prisoner))]
        public int PrisonerId  { get; set; }
        [Required]
        public Prisoner Prisoner { get; set; }


        [ForeignKey(nameof(Officer))]
        public int OfficerId { get; set; }
        [Required]
        public Officer Officer { get; set; }
    }
}