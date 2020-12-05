using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ADO.Net.Examples
{
    public static partial class DBFunctions
    {
        private static DataTable GetTVP(IEnumerable<string> cities)
        {
            var table = new DataTable();

            table.Columns.Add(new DataColumn
            {
                DataType = Type.GetType("System.String"),
                ColumnName = "city"
            });

            foreach (var city in cities)
            {
                var row = table.NewRow();
                row["city"] = city;
                table.Rows.Add(row);
            }

            return table;
        }

        public static void ExecuteStoredProcedureWithTableValueParameter(string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sales.get_employees_by_stores", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@cities", GetTVP(new[] { "Baldwin", "Rowlett", "Santa Cruz" }));

                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"First Name: {reader["first_name"]}, Last Name: {reader["last_name"]}, City: {reader["city"]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
