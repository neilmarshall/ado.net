using System;
using System.Data;
using System.Data.SqlClient;

namespace ADO.Net.Examples
{
    public static partial class DBFunctions
    {
        public static void GetQueryAsDataTable(string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand(
                    "SELECT TOP 4 p.product_name, p.model_year, p.list_price, b.brand_name, c.category_name FROM production.products p JOIN production.brands b ON p.brand_id = b.brand_id JOIN production.categories c ON p.category_id = c.category_id;",
                    conn);
                using var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);  // the 'Fill' method handles the opening (and closing) of the connection
                var index = 1;
                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine($"Parsing row {index}");
                    foreach (DataColumn col in dt.Columns)
                    {
                        Console.WriteLine($"\t{col.ColumnName}: {row[col.ColumnName].ToString()}");
                    }
                    index++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
