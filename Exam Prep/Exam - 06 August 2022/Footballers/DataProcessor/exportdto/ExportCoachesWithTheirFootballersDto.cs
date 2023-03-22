using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{

    [XmlType("Coach")]
    public class ExportCoachesWithTheirFootballersDto
    {
        [XmlAttribute("FootballersCount")]
        public int CoachFootballersCount { get; set; }

        [XmlElement("CoachName")]
        public string CoachName { get; set; }

        [XmlArray("Footballers")]
        public FootballerDto[] Footballers { get; set; } 
    }

    [XmlType("Footballer")]
    public class FootballerDto 
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }
    }
}
