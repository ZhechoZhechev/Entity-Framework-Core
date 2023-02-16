

using Microsoft.Data.SqlClient;

SqlConnection connection = new SqlConnection("Server=.;Database=SoftUni;User Id=sa;Password=8201291108;Trusted_Connection=False;MultipleActiveResultSets=true; TrustServerCertificate=true");

connection.Open();

using (connection) 
{
    SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE DepartmentId = 5", connection);

    SqlDataReader reader = cmd.ExecuteReader();

    using (reader)
    {
        while (reader.Read())
        {
            string? firstName = reader["FirstName"].ToString();
            string? lastName = reader["LastName"].ToString();
            decimal salary = (decimal)reader["Salary"];

            Console.WriteLine($"{firstName} {lastName} has ${salary} salary");
        }
    }
}

