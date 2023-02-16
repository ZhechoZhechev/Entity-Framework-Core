
using _02.VillainNames;
using Microsoft.Data.SqlClient;
using System.Text;

SqlConnection connection = new SqlConnection(Config.ConnectionString);

connection.Open ();

string sqlCode =
    @"SELECT
       	      v.[Name],
       COUNT(mv.[MinionId]) AS [MinionCount]
       	   FROM [Villains] AS v
           JOIN [MinionsVillains] AS mv ON mv.[VillainId] = v.[Id]
     GROUP BY v.[Id], v.[Name]
HAVING COUNT(mv.[MinionId]) > 3
       ORDER BY [MinionCount] DESC";

SqlCommand cmd = new SqlCommand(sqlCode, connection);

StringBuilder sb = new StringBuilder ();

using (connection)
{
    SqlDataReader reader = cmd.ExecuteReader ();

    while (reader.Read ()) 
    {
        sb.AppendLine($"{reader["Name"]} - {reader["MinionCount"]}");
    }

}
Console.WriteLine(sb.ToString().TrimEnd());