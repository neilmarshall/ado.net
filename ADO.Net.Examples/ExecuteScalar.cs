using System;
using System.Data.SqlClient;

namespace ADO.Net.Examples
{
    public static partial class DBFunctions
    {
        /// <summary>
        /// The 'ExecuteScalar' method executes the query, and returns the first
        /// column of the first row in the result set returned by the query (additional
        /// columns or rows are ignored
        /// </summary>
        public static void ExecuteScalar(string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("SELECT COUNT(*) FROM production.products;", conn);
                conn.Open();
                var count = (int)cmd.ExecuteScalar();
                Console.WriteLine($"Total number of products: {count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
