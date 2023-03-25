namespace TeisterMask.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Xml.Serialization;

    using AutoMapper;

    using Newtonsoft.Json;

    using Data;
    using Data.Models;
    using ExportDto;

    using Formatting = Newtonsoft.Json.Formatting;
    using System.Xml;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projTaskDtos = context.Projects
                .Where(p => p.Tasks.Any())
                .OrderByDescending(p => p.Tasks.Count())
                .ThenBy(p => p.Name)
                .Select(p => new ExportProjectsTasksDto() 
                {
                    TasksCount = p.Tasks.Count().ToString(),
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate == null ? "No" : "Yes",
                    Tasks = p.Tasks
                    .Select(t => new TasksDto() 
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    })
                    .OrderBy(t => t.Name)
                    .ToArray()
                })
                .ToArray();

            var output = Serialize<ExportProjectsTasksDto[]>(projTaskDtos, "Projects");
            return output;
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employeesDtos = context.Employees
                .Where(e => e.EmployeesTasks.Any(t => t.Task.OpenDate >= date))
                .ToArray()
                .Select(e => new
                {
                    e.Username,
                    Tasks = e.EmployeesTasks
                    .Where(t => t.Task.OpenDate >= date)
                    .OrderByDescending(t => t.Task.DueDate)
                    .ThenBy(t => t.Task.Name)
                    .Select(t => new
                    {
                        TaskName = t.Task.Name,
                        OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = t.Task.LabelType.ToString(),
                        ExecutionType = t.Task.ExecutionType.ToString()
                    })
                    .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Count())
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            var output = JsonConvert.SerializeObject(employeesDtos, Formatting.Indented);
            return output;
        }

        private static string Serialize<T>(T dataTransferObjects, string xmlRootAttributeName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttributeName));
            
            StringBuilder sb = new StringBuilder();

            using var write = new SWEncodingCustom(sb);


            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(write, dataTransferObjects, xmlNamespaces);
            return sb.ToString();
        }

        public class SWEncodingCustom : StringWriter
        {
            public SWEncodingCustom(StringBuilder sb) :base(sb)
            {
            }
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}