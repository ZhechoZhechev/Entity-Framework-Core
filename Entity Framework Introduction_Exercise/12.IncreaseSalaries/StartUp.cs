namespace SoftUni;

using Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class StartUp
{
    static void Main()
    {
        var context = new SoftUniContext();
        string output = IncreaseSalaries(context);
        Console.WriteLine(output);

    }

    public static string IncreaseSalaries(SoftUniContext context)
    {
        var depNames = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

        var empToIncreaseSalaryTo = context.Employees
            .Where(x => depNames.Contains(x.Department.Name))
            .ToArray();

        foreach (var emp in empToIncreaseSalaryTo)
        {
            emp.Salary *= 1.12m;
        }

        context.SaveChanges();

        var employeesInfo = context.Employees
            .Where(x => depNames.Contains(x.Department.Name))
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .Select(x => new
            {
                x.FirstName,
                x.LastName,
                x.Salary
            })
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var e in employeesInfo)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
        }

        return sb.ToString().TrimEnd();
    }
}