using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ADO.Net.Examples
{
    public static partial class DBFunctions
    {
        /// <summary>
        /// The 'ExecuteNonQueryAsync' method can be used to execute a stored procedure with
        /// an output parameter asynchronously
        /// </summary>
        public static async Task ExecuteStoredProcedureWithOutputParameterAsync(string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sales.store_count", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@count", SqlDbType.Int) { Direction = ParameterDirection.Output });
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine($"Total number of stores: {(int)cmd.Parameters["@count"].Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
