using SoftUni.Data;

namespace SoftUni;

internal class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext context = new SoftUniContext();
        Console.WriteLine("Connection success !");
    }
}