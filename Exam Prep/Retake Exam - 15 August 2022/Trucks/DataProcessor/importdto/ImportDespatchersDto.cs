namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Despatcher")]
    public class ImportDespatchersDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlArray("Trucks")]
        public ImportTrucksDto[] Trucks { get; set; }
    }

    [XmlType("Truck")]
    public class ImportTrucksDto 
    {
        [XmlElement("RegistrationNumber")]
        [Required]
        [StringLength(8)]
        [RegularExpression(@"^[A-Z]{2}\d{4}[A-Z]{2}$")]
        public string RegistrationNumber { get; set; }

        [XmlElement("VinNumber")]
        [Required]
        [StringLength(17)]
        public string VinNumber { get; set; }

        [XmlElement("TankCapacity")]
        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [XmlElement("CargoCapacity")]
        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }

        [XmlElement("CategoryType")]
        [Required]
        public int CategoryType { get; set; }

        [XmlElement("MakeType")]
        [Required]
        public int MakeType { get; set; }

    }
}
