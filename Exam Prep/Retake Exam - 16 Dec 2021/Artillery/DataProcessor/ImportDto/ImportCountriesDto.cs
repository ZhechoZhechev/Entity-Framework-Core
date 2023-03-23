using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class ImportCountriesDto
    {
        [XmlElement("CountryName")]
        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        public string CountryName { get; set; }

        [XmlElement("ArmySize")]
        [Required]
        [Range(50000, 10000000)]
        public int ArmySize { get; set; }
    }
}
