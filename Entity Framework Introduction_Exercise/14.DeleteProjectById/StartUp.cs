namespace SoftUni;

using Data;

public class StartUp
{
    static void Main()
    {
        var context = new SoftUniContext();
        string output = DeleteProjectById(context);
        Console.WriteLine(output);
    }

    public static string DeleteProjectById(SoftUniContext context) 
    {
        var empProjectsToDel = context.EmployeesProjects.Where(x => x.ProjectId == 2);
        context.EmployeesProjects.RemoveRange(empProjectsToDel);

        var projToDel = context.Projects.Where(x => x.ProjectId == 2);
        context.Projects.RemoveRange(projToDel);

        context.SaveChanges();

        var tenProjects = context.Projects
            .Take(10)
            .Select(x => x.Name)
            .ToArray();


        return string.Join(Environment.NewLine, tenProjects);
    }
}