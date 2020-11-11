using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ADO.Net.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AzureDBConnection"].ConnectionString;
            ExecuteScalar(connectionString);
            await ExecuteScalarAsync(connectionString);
        }

        /// <summary>
        /// The 'ExecuteScalar' method executes the query, and returns the first
        /// column of the first row in the result set returned by the query (additional
        /// columns or rows are ignored
        /// </summary>
        static void ExecuteScalar(string connectionString)
        {
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand("SELECT COUNT(*) FROM production.products;", conn);
            conn.Open();
            var count = (int)cmd.ExecuteScalar();
            Console.WriteLine(count);
        }

        /// <summary>
        /// The 'ExecuteScalarAsync' method executes the query asynchronously, and returns the first
        /// column of the first row in the result set returned by the query (additional
        /// columns or rows are ignored
        /// </summary>
        static async Task ExecuteScalarAsync(string connectionString)
        {
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand("SELECT COUNT(*) FROM production.stocks;", conn);
            conn.Open();
            var count = (int)(await cmd.ExecuteScalarAsync());
            Console.WriteLine(count);
        }
    }
}
