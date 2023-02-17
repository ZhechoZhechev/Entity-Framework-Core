using _02.VillainNames;
using Microsoft.Data.SqlClient;

connection = new SqlConnection(Config.ConnectionString);

string[] minInfo = Console.ReadLine().Split(" ").Skip(1).ToArray();

string minionName = minInfo[0];
int age = int.Parse(minInfo[1]);
string minionTown = minInfo[2];

string[] villainInfo = Console.ReadLine()?.Split(" ").Skip(1).ToArray();
string villainName = villainInfo[0];

using (connection)
{
    connection.Open();
    AddMinionTOVillain(minionTown, villainName, minionName, age);
    connection.Close();
}

static void AddMinionTOVillain(string minionTown, string villainName, string minionName, int minionAge)
{
    int townId = GetTownIdAndAddTown(minionTown);

    string insertMinion = @$"INSERT INTO [Minions] VALUES('{minionName}', '{minionAge}', '{townId}')";
    SqlCommand insertMinionIntoBase = new SqlCommand(insertMinion, connection);
    insertMinionIntoBase.ExecuteNonQuery();

    int minionId = GetMinionId(minionName);
    int villainId = GetVillainIdAndAddVillain(villainName);

    string insretMinionToVillain = @$"INSERT INTO [MinionsVillains] VALUES('{minionId}', '{villainId}')";

    Console.WriteLine(@$"Successfully added {minionName} to be minion of {villainName}.");
}

static int GetTownIdAndAddTown(string minionTown)
{
    string townQ = @"SELECT [Id] FROM [Towns] WHERE [Name] = @minionTown";
    SqlCommand getTownId = new SqlCommand(townQ, connection);
    getTownId.Parameters.AddWithValue("@minionTown", minionTown);

    object Id = getTownId.ExecuteScalar();
    if (Id != null)
    {
        return (int)Id;
    }

    string addTown = @$"INSERT INTO [Towns] VALUES('{minionTown}', NULL)";
    SqlCommand addTownToBase = new SqlCommand(addTown, connection);
    addTownToBase.ExecuteNonQuery();
    Console.WriteLine($"Town {minionTown} was added to the database");

    Id = getTownId.ExecuteScalar();
    return (int)Id;
}

static int GetVillainIdAndAddVillain(string villainName)
{
    string villainQ = @"SELECT [Id] FROM [Villains] WHERE [Name] = @villainName";
    SqlCommand getVillainId = new SqlCommand(villainQ, connection);
    getVillainId.Parameters.AddWithValue("@villainName", villainName);

    object Id = getVillainId.ExecuteScalar();
    if (Id != null)
    {
        return (int)Id;
    }

    string addVillain = $@"INSERT INTO [Villains] VALUES('{villainName}', 4)";
    SqlCommand addVillainToBase = new SqlCommand(addVillain, connection);
    addVillainToBase.ExecuteNonQuery();
    Console.WriteLine($"Villain {villainName} was added to the database.");

    Id = getVillainId.ExecuteScalar();
    return (int)Id;
}

static int GetMinionId(string minionName)
{
    string minionQ = @"SELECT [Id] FROM [Minions] WHERE [Name] = @minionName";
    SqlCommand findMinionId = new SqlCommand(minionQ, connection);
    findMinionId.Parameters.AddWithValue("@minionName", minionName);
    object Id = findMinionId.ExecuteNonQuery();

    return (int)Id;
}

public partial class Program
{
    private static SqlConnection? connection;
}
