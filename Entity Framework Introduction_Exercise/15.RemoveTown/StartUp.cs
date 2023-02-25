namespace SoftUni;

using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SoftUni.Data;

public class StartUp
{
    static void Main()
    {
        var context = new SoftUniContext();
        string output = RemoveTown(context);
        Console.WriteLine(output);
    }

    public static string RemoveTown(SoftUniContext context) 
    {
        var townToDel = context.Towns
            .Where(x => x.Name == "Seattle")
            .FirstOrDefault();

        var addressesToDel = context.Addresses
            .Where(x => x.TownId == townToDel.TownId)
            .ToArray();

        var empToSetAddressToNull = context.Employees
            .Where(x => addressesToDel
            .Contains(x.Address))
            .ToArray();

        foreach (var e in empToSetAddressToNull) 
        {
            e.AddressId = null;
        }

        context.Towns.Remove(townToDel);
        context.Addresses.RemoveRange(addressesToDel);

        context.SaveChanges();

        return $"{addressesToDel.Count()} addresses in Seattle were deleted";
    }
}