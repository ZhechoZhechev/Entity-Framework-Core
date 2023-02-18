using _02.VillainNames;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

connection = new SqlConnection(Config.ConnectionString);

int minionId = int.Parse(Console.ReadLine());

using (connection)
{
    connection.Open();
    CreateProcedure(@"CREATE OR ALTER PROC usp_GetOlder(@Id INT) AS UPDATE Minions SET Age += 1 WHERE Id = @Id");
    IncreaseAgeByOne(minionId);
    string result = GetResultString(minionId);
    connection.Close();
    Console.WriteLine(result);
}
        

 static void CreateProcedure(string query)
{
    SqlCommand cmd = new SqlCommand(query, connection);
    cmd.ExecuteNonQuery();
}

static void IncreaseAgeByOne(int id)
{
    SqlCommand cmd = new SqlCommand(@"EXEC usp_GetOlder @Id", connection);
    cmd.Parameters.AddWithValue("@Id", id);
    cmd.ExecuteNonQuery();
}

static string GetResultString(int minionId)
{
    string queryText = @"SELECT Name, Age FROM Minions WHERE Id = @Id";
    SqlCommand cmd = new SqlCommand(queryText, connection);
    cmd.Parameters.AddWithValue("@Id", minionId);

    using SqlDataReader reader = cmd.ExecuteReader();
    reader.Read();
    string name = (string)reader["Name"];
    int age = (int)reader["Age"];
    reader.Close();

    return $"{name} - {age} years old";
}
public partial class Program
{
    private static SqlConnection? connection;
}
