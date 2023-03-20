namespace SoftJail.DataProcessor.ExportDto
{
using System.Xml.Serialization;

    [XmlType("Prisoner")]
    public class ExportPrisonersInboxDto
    {
        [XmlElement("Id")]
        public int PrisonerId { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; }

        [XmlArray("EncryptedMessages")]
        public Message[] EncryptedMessages { get; set; }
    }

    [XmlType("Message")]
    public class Message 
    {
        [XmlElement("Description")]
        public string Description { get; set; }
    }
}
