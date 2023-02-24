using SoftUni.Data;
using SoftUni.Models;


namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            string result = AddNewAddressToEmployee(context);
            Console.WriteLine(result);
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {

            Address address = new Address();
            address.AddressText = "Vitoshka 15";
            address.TownId = 4;

            context.Addresses.Add(address);

            var nakovEmpl = context.Employees.Where(e => e.LastName == "Nakov").First();

            nakovEmpl.Address = address;
            context.SaveChanges();

            var employeesTextsOrdered = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address.AddressText)
                .Take(10)
                .ToList();

            return string.Join(Environment.NewLine, employeesTextsOrdered);
        }
    }
}