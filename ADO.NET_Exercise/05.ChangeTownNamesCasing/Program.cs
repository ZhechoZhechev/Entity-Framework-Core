using _02.VillainNames;
using Microsoft.Data.SqlClient;

connection = new SqlConnection(Config.ConnectionString);

string countryName = Console.ReadLine();

using (connection) 
{
    connection.Open();
    ChangeTownsToUpper(countryName);
    connection.Close();
}

static int? FindCountryId(string countryName) 
{
    string idQuery = @"SELECT [Id] FROM [Countries] WHERE [Name] = @countryName";
    SqlCommand cmd = new SqlCommand(idQuery, connection);
    cmd.Parameters.AddWithValue("@countryName", countryName);

    object result = cmd.ExecuteScalar();
    if (result == null)
        return null;
    return (int?)result;
}

static void ChangeTownsToUpper(string countryName) 
{

    if (FindCountryId(countryName) == null) 
    {
        Console.WriteLine("No town names were affected.");
        return;
    }
        

    int CountryId = (int)FindCountryId(countryName);

    string toUpperQuery = @$"UPDATE [Towns] SET [Name] = UPPER([Name]) WHERE [CountryCode] = {CountryId}";
    SqlCommand cmd = new SqlCommand(toUpperQuery, connection);
    cmd.ExecuteNonQuery();

    string namesQuery = @$"SELECT [Name] FROM [Towns] WHERE [CountryCode] = {CountryId}";
    SqlCommand readCmd = new SqlCommand(namesQuery, connection);
    SqlDataReader reader = readCmd.ExecuteReader();

    List<string> result = new List<string>();

    while (reader.Read()) 
    {
        result.Add(reader["Name"].ToString());
    }

    Console.WriteLine($"{result.Count} town names were affected.");
    Console.WriteLine(string.Join(", ", result));
}

public partial class Program
{
    private static SqlConnection? connection;
}
