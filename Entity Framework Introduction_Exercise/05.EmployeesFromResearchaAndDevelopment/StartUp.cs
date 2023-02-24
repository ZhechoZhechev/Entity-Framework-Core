using SoftUni.Data;
using System.Text;

namespace SoftUni;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext context = new SoftUniContext();
        string result = GetEmployeesFromResearchAndDevelopment(context);
        Console.WriteLine(result);
    }

    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context) 
    {
        var employees = context.Employees
            .Where(e => e.Department.Name == "Research and Development")
            .OrderBy(e => e.Salary)
            .ThenByDescending(e => e.FirstName)
            .Select(e => new {e.FirstName, e.LastName, DepartmentName = e.Department.Name, e.Salary })
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var e in employees) 
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:F2}");
        }

        return sb.ToString().TrimEnd();
    }
}