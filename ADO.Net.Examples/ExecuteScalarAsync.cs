using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ADO.Net.Examples
{
    public static partial class DBFunctions
    {
        /// <summary>
        /// The 'ExecuteScalarAsync' method executes the query asynchronously, and returns the first
        /// column of the first row in the result set returned by the query (additional
        /// columns or rows are ignored
        /// </summary>
        public static async Task ExecuteScalarAsync(string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("SELECT COUNT(*) FROM production.stocks;", conn);
                conn.Open();
                var count = (int)(await cmd.ExecuteScalarAsync());
                Console.WriteLine($"Total number of stocks: {count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
