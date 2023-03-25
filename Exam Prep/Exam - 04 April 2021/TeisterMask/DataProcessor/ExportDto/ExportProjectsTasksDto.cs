namespace TeisterMask.DataProcessor.ExportDto
{
using System.Xml.Serialization;
    
    [XmlType("Project")]
    public class ExportProjectsTasksDto
    {
        [XmlAttribute("TasksCount")]
        public string TasksCount { get; set; }

        [XmlElement("ProjectName")]
        public string ProjectName { get; set;}

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set;}

        [XmlArray]
        public TasksDto[] Tasks { get; set;}
    }

    [XmlType("Task")]
    public class TasksDto 
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Label")]
        public string Label { get; set; }
    }
}
