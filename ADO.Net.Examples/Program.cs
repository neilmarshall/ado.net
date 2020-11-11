using System;
using System.Configuration;
using System.Data;
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
            await Task.WhenAll(
                ExecuteScalarAsync(connectionString),
                ExecuteStoredProcedureWithOutputParameterAsync(connectionString));
            ExecuteReader(connectionString);
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
            Console.WriteLine($"Total number of products: {count}");
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
            Console.WriteLine($"Total number of stocks: {count}");
        }

        /// <summary>
        /// The 'ExecuteNonQueryAsync' method can be used to execute a stored procedure with
        /// an output parameter asynchronously
        /// </summary>
        static async Task ExecuteStoredProcedureWithOutputParameterAsync(string connectionString)
        {
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand("sales.store_count", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@count", SqlDbType.Int) { Direction = ParameterDirection.Output });
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
            Console.WriteLine($"Total number of stores: {(int)cmd.Parameters["@count"].Value}");
        }

        /// <summary>
        /// The 'ExecuteReader' methods creates a data reader (a forward-only cursor) that
        /// can be used to iterate through one or more result sets
        /// </summary>
        static void ExecuteReader(string connectionString)
        {
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand("SELECT store_name FROM sales.stores;", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                Console.WriteLine($"\tStore: {reader["store_name"]}");
            }
        }
    }
}
