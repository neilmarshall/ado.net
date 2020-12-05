using System;
using System.Data;
using System.Data.SqlClient;

namespace ADO.Net.Examples
{
    public static partial class DBFunctions
    {
        /// <summary>
        /// The 'ExecuteReader' method creates a data reader (a forward-only cursor) that
        /// can be used to iterate through one or more result sets
        /// </summary>
        public static void ExecuteReader(string connectionString)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
