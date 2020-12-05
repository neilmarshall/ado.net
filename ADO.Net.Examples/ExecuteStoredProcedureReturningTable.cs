using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ADO.Net.Examples
{
    public static partial class DBFunctions
    {
        /// <summary>
        /// We can also execute a stored procedure that does not have an output
        /// parameter and instead simply returns a result set
        /// </summary>
        public static async Task ExecuteStoredProcedureReturningTable(string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sales.get_employees_by_store", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar, 255) { Value = "Rowlett" });
                conn.Open();
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    Console.WriteLine($"\tFirst Name: {reader["first_name"]}");
                    Console.WriteLine($"\tLast Name: {reader["last_name"]}");
                    Console.WriteLine($"\tCity: {reader["city"]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
