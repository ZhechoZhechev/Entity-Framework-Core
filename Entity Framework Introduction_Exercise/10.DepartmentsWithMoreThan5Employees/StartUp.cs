namespace SoftUni;

using Data;
using System.Text;

public class StartUp
{
    static void Main(string[] args)
    {
        var context = new SoftUniContext();
        string output = GetDepartmentsWithMoreThan5Employees(context);
        Console.WriteLine(output);
    }

    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context) 
    {

        var departments = context.Departments.
            Where(x => x.Employees.Count > 5)
            .OrderBy(x => x.Employees.Count)
            .ThenBy(x => x.Name)
            .Select(d => new
            {
                d.Name,
                ManagerFirstN = d.Manager.FirstName,
                ManagerLastN = d.Manager.LastName,
                DepEmployees = d.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle
                })
                .ToArray()
            })
            .ToArray();

        StringBuilder sb = new StringBuilder();
        foreach (var d in departments) 
        {
            sb.AppendLine($"{d.Name} - {d.ManagerFirstN}  {d.ManagerLastN}");

            foreach (var e in d.DepEmployees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
            }
        }
        return sb.ToString().TrimEnd();
    }

}