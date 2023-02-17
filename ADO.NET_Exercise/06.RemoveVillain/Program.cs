using _02.VillainNames;
using Microsoft.Data.SqlClient;

connection = new SqlConnection(Config.ConnectionString);

int villainId = int.Parse(Console.ReadLine());

using (connection)
{
    connection.Open();
    string villainName = GetVillainName(villainId);
    int minionsCount = GetMinionsCount(villainId);

    if (villainName == string.Empty) 
    {
        Console.WriteLine("No such villain was found.");
        return;
    }
    else 
    {
        Console.WriteLine($"{villainName} was deleted.");
    }

    Console.WriteLine($"{minionsCount} minions were released.");

    DeleteFromMinionsVillains(villainId);
    DeleteFromVillains(villainId);
    connection.Close();
}

static string GetVillainName(int villainId)
{
    string nameById = @"SELECT [Name] FROM [Villains] WHERE [Id] = @villainId";
    SqlCommand cmd = new SqlCommand(nameById, connection);
    cmd.Parameters.AddWithValue("@villainId", villainId);
    object villainName = cmd.ExecuteScalar();
    if (villainName == null)
        return string.Empty;
    return villainName.ToString();
}

static int GetMinionsCount(int villainId)
{
    string minionsCount = @"SELECT COUNT([MinionId]) FROM [MinionsVillains] WHERE [VillainId] = @villainId";
    SqlCommand cmd = new SqlCommand(minionsCount, connection);
    cmd.Parameters.AddWithValue("@villainId", villainId);
    object result = cmd.ExecuteScalar();

    if (result == null)
        return 0;
    return (int)result;
}

static void DeleteFromMinionsVillains(int villainId)
{

    string delFromMV = @"DELETE FROM [MinionsVillains] WHERE [VillainId] = @villainId";
    SqlCommand cmd = new SqlCommand(delFromMV, connection);
    cmd.Parameters.AddWithValue("@villainId", villainId);
    cmd.ExecuteNonQuery();
}

static void DeleteFromVillains(int villainId) 
{

    string delFromVillains = @$"DELETE FROM [Villains] WHERE [Id] = @villainId";
    SqlCommand cmd = new SqlCommand(delFromVillains, connection);
    cmd.Parameters.AddWithValue("@villainId", villainId);
    cmd.ExecuteNonQuery();
}

public partial class Program
{
    private static SqlConnection? connection;
}