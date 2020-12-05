using System;
using System.Data;
using System.Data.SqlClient;

namespace ADO.Net.Examples
{
    public static partial class DBFunctions
    {
        /// <summary>
        /// The 'NextResult' method on the DataReader class is used to iterate through
        /// subsequent result sets
        /// </summary>
        public static void ExecuteReaderWithMultipleResultSets(string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("SELECT store_name FROM sales.stores; SELECT manager_id FROM sales.staffs;", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    Console.WriteLine($"\tStore: {reader["store_name"]}");
                }
                reader.NextResult();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("manager_id")))
                    {
                        Console.WriteLine($"\tManager ID: {(int)reader["manager_id"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
