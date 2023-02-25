
namespace SoftUni;

using Data;

public class StartUp
{
    static void Main(string[] args)
    {
        var context = new SoftUniContext();
        string output = GetEmployee147(context);
        Console.WriteLine(output);
    }

    public static string GetEmployee147(SoftUniContext context) 
    {
        var employee = context.Employees
            .Where(x => x.EmployeeId == 147)
            .Select(e => new 
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                Projects = e.EmployeesProjects
                .Select(p => new {
                    ProjectName = p.Project.Name
                })
                .OrderBy(p => p.ProjectName)
                .ToArray()

            })
            .FirstOrDefault();

        return $"{employee.FirstName} {employee.LastName} - {employee.JobTitle}" + Environment.NewLine +
            string.Join(Environment.NewLine, employee.Projects.Select(p => p.ProjectName));
    }
}