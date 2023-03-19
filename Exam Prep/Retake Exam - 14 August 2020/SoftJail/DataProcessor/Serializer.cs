namespace SoftJail.DataProcessor
{

    using Data;
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            throw new NotImplementedException();
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            throw new NotImplementedException();
        }

        private static string Serialize<T>(T dtos, string rootAtributeName)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootAtributeName);
            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            xmlns.Add(string.Empty, string.Empty);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringWriter swriter = new StringWriter(sb);

            xmlSerializer.Serialize(swriter, dtos, xmlns);
            return sb.ToString();
        }
    }

}