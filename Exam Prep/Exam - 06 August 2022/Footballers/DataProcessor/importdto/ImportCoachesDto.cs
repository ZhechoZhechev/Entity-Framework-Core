namespace Footballers.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Coach")]
    public class ImportCoachesDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [XmlElement("Nationality")]
        [Required]
        public string Nationality { get; set; }

        [XmlArray("Footballers")]
        public FootballerDto[] Footballers { get; set; }
    }

    [XmlType("Footballer")]
    public class FootballerDto 
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [XmlElement("ContractStartDate")]
        [Required]
        public string ContractStartDate { get; set; }

        [XmlElement("ContractEndDate")]
        [Required]
        public string ContractEndDate { get; set; }

        [Required]
        public int BestSkillType { get; set; }

        [Required]
        public int PositionType { get; set; }
    }
}
