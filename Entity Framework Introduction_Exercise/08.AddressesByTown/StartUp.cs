
namespace SoftUni;

using System.Text;

using SoftUni.Data;
public class StartUp
{
    static void Main(string[] args)
    {
        var context = new SoftUniContext();
        string output = GetAddressesByTown(context);
        Console.WriteLine(output);
    }

    public static string GetAddressesByTown(SoftUniContext context) 
    {
        var addresses = context.Addresses
            .OrderByDescending(x => x.Employees.Count)
            .ThenBy(x => x.Town.Name)
            .ThenBy(x => x.AddressText)
            .Take(10)
            .Select(x => new {x.AddressText,TownName = x.Town.Name, EmpCount = x.Employees.Count } )
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach ( var a in addresses ) 
        {
            sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmpCount} employees");
        }

        return sb.ToString().TrimEnd();
    }
}