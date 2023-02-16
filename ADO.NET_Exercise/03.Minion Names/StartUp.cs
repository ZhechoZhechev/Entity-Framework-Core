using _02.VillainNames;
using Microsoft.Data.SqlClient;
using System.Text;

connection = new SqlConnection(Config.ConnectionString);


Console.Write("Villain ID = ");
int villainId = int.Parse(Console.ReadLine());

using (connection)
{
    connection.Open();
    string result = GetMinionsInfoByVillain(villainId);

    Console.WriteLine(result);
}

static string GetMinionsInfoByVillain(int villainId) 
{
    string villainName = GetVillainName(villainId);
    if (villainName == null)
    {
        return $"No villain with ID <{villainId}> exists in the database.";
    }

    string minionsInfo = GetMinionsNamesAndAge(villainId);
    if (minionsInfo == null)
    {
        return $"Villain: {villainName}{Environment.NewLine}(no minions)";
    }

    return $"Villain: {villainName}{Environment.NewLine}{minionsInfo}";
}

static string GetVillainName(int villainId) 
{
    string ifVillainExists = @"SELECT [Name] FROM Villains WHERE [Id] = @villainId";
    SqlCommand getVillainName = new SqlCommand(ifVillainExists,connection);
    getVillainName.Parameters.AddWithValue("@villainId", villainId);
    return (string)getVillainName.ExecuteScalar();
}

static string GetMinionsNamesAndAge(int villainId) 
{
    string minionNamesQ =
     @"SELECT 
             m.[Name],
             m.[Age]
          FROM [Villains] AS v
     LEFT JOIN [MinionsVillains] mv ON mv.[VillainId] = v.[Id]
     LEFT JOIN [Minions] AS m ON mv.[MinionId] = m.[Id]
       WHERE v.[Id] = @villainId
    ORDER BY m.[Name]";

    SqlCommand getNamesAndAges  = new SqlCommand(minionNamesQ,connection);
    getNamesAndAges.Parameters.AddWithValue("@villainId", villainId);
    SqlDataReader reader = getNamesAndAges.ExecuteReader();
    StringBuilder sb = new StringBuilder();

    int minionCounter = 1;
    while (reader.Read())
    {
        sb.AppendLine($"{minionCounter++}. {reader["Name"]} {reader["Age"]}");
    }
    return sb.ToString().TrimEnd();
}

public partial class Program
{
    private static SqlConnection? connection;
}


