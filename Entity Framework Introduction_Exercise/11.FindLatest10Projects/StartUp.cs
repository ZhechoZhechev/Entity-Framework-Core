namespace SoftUni;

using Data;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

public class StartUp
{
    static void Main()
    {
        var context = new SoftUniContext();
        string output = GetLatestProjects(context);
        Console.WriteLine(output);
    }

    public static string GetLatestProjects(SoftUniContext context)
    {
        var projects = context.Projects
            .OrderByDescending(p => p.StartDate)
            .Take(10)
            .OrderBy(p => p.Name)
            .Select(p => new
            {
                p.Name,
                p.Description,
                StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
            })
            .ToArray();
        StringBuilder sb = new StringBuilder();

        foreach (var p in projects)
        {
            sb.AppendLine(p.Name)
              .AppendLine(p.Description)
              .AppendLine(p.StartDate);
        }

        return sb.ToString().TrimEnd();
    }
}