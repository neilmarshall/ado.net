using System;
using System.Data;
using System.Data.SqlClient;

namespace ADO.Net.Examples
{
    public static partial class DBFunctions
    {
        /// <summary>
        /// The 'IsDBNull' method on the DataReader class checks if a value is
        /// null - this is useful before attempting to cast to a non-nullable type
        /// </summary>
        public static void ExecuteReaderWithTypeConversionAndNullChecking(string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("SELECT manager_id FROM sales.staffs;", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
