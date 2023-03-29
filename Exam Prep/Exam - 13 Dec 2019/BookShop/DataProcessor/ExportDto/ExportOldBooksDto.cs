using System;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book")]
    public class ExportOldBooksDto
    {
        [XmlAttribute("Pages")]
        public int Pages { get; set; }

        [XmlElement("Name")]
        public string BookName { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }
    }
}
