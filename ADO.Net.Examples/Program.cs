using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ADO.Net.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AzureDBConnection"].ConnectionString;
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand("SELECT COUNT(*) FROM production.products;", conn);
            conn.Open();
            var count = (int)cmd.ExecuteScalar();
            Console.WriteLine(count);
        }
    }
}
