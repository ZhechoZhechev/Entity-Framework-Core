using _02.VillainNames;
using Microsoft.Data.SqlClient;

connection = new SqlConnection(Config.ConnectionString);

var minionIds = Console.ReadLine().Split(" ").Select(int.Parse).ToList();

var output = new Dictionary<string, int>();

using (connection)
{
    connection.Open();
    IncreaseAgeById(minionIds);
    output = ReturnData();
    connection.Close();
}

foreach (var minion in output)
{
    Console.WriteLine($"{minion.Key} {minion.Value}");
}


static Dictionary<string, int> ReturnData()
{
    var output = new Dictionary<string, int>();

    string sqlCmd = @"SELECT [Name], [Age] from [Minions]";
    SqlCommand cmd = new SqlCommand(sqlCmd, connection);
    SqlDataReader reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        output.Add(reader["Name"].ToString(), (int)reader["Age"]);
    }

    return output;
}
static void IncreaseAgeById(List<int> ids)
{
    for (int i = 0; i < ids.Count; i++)
    {
        string slqCmd = @"UPDATE [Minions] SET [Name] = LOWER(LEFT([Name], 1)) + SUBSTRING(Name, 2, LEN([Name])), [Age] += 1 WHERE Id = @Id";
        SqlCommand cmd = new SqlCommand(slqCmd, connection);
        cmd.Parameters.AddWithValue("@Id", ids[i]);
        cmd.ExecuteNonQuery();

    }
}

public partial class Program
{
    private static SqlConnection? connection;
}