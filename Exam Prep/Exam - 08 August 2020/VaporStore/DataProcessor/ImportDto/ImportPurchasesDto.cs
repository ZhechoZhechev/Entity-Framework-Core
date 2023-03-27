using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ImportDto
{
    [XmlType("Purchase")]
    public class ImportPurchasesDto
    {
        [XmlAttribute("title")]
        [Required]
        public string GameName { get; set; }

        [XmlElement("Type")]
        [Required] 
        public string Type { get; set; }

        [XmlElement("Key")]
        [Required]
        [RegularExpression(@"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        public string ProductKey { get; set; }

        [XmlElement("Card")]
        [Required]
        public string CardNumber { get; set; }

        [XmlElement("Date")]
        [Required]
        public string Date { get; set; }

    }
}
