namespace SoftJail.DataProcessor.ImportDto
{
using System.ComponentModel.DataAnnotations;

    public class ImportPrisonersMailsDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^The [A-Z][a-z]*$")]
        public string Nickname { get; set; }

        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; }

        public string ReleaseDate { get; set; }

        [Range(typeof(decimal),"0", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }

        public int? CellId { get; set; }

        public ImportPrisonerMailDto[] Mails { get; set; }
    }

    public class ImportPrisonerMailDto 
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]*str\.$")]
        public string Address { get; set; }
    }
}
