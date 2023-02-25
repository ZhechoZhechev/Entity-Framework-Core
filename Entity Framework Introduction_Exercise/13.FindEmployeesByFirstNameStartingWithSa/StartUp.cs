namespace SoftUni;

using Data;
using System.Text;

public class StartUp
{
    static void Main()
    {
        var context = new SoftUniContext();
        string output = GetEmployeesByFirstNameStartingWithSa(context);
        Console.WriteLine(output);
    }
    public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context) 
    {
        var employees = context.Employees
            .Where(e => e.FirstName.StartsWith("Sa"))
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                Salary = $"${e.Salary:f2}"
            })
            .ToArray();

        StringBuilder sb = new StringBuilder();
        foreach (var e in employees) 
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - ({e.Salary})");
        }
        return sb.ToString().TrimEnd();
    }
}