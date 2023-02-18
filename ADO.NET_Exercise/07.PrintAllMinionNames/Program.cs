using _02.VillainNames;
using Microsoft.Data.SqlClient;

connection = new SqlConnection(Config.ConnectionString);

var minions = new List<string>();

using (connection) 
{
    connection.Open();
    minions = ExtractAllMinions();
    connection.Close();
}

var rearrenged =  RearrangeNames(minions);

rearrenged.ForEach(x =>Console.WriteLine(x));

//foreach (var name in rearrenged) 
//{
//    Console.WriteLine(name);
//}
static List<string> RearrangeNames(List<string> minions) 
{
    var rearranged = new List<string>();

    int counter = 0;
    while (minions.Any()) 
    {
        if (counter % 2 == 0)
        {
            rearranged.Add(minions.First());
            minions.RemoveAt(0);
        }
        else
        {
            rearranged.Add(minions.Last());
            minions.RemoveAt(minions.Count - 1);
        }
        counter++;
    }
    return rearranged;
}

static List<string> ExtractAllMinions() 
{
    var minions = new List<string>();

    string sqlCode = @"SELECT [Name] FROM [Minions]";
    SqlCommand cmd = new SqlCommand(sqlCode, connection);

    SqlDataReader reader = cmd.ExecuteReader();

    while (reader.Read()) 
    {
        minions.Add(reader["Name"].ToString());
    }

    return minions;
}
public partial class Program
{
    private static SqlConnection? connection;
}