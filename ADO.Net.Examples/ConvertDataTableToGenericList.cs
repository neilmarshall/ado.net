using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ADO.Net.Examples
{
    public static partial class DBFunctions
    {
        public class DetailedProduct
        {
            public string ProductName { get; set; }
            public int ModelYear { get; set; }
            public decimal ListPrice { get; set; }
            public string BrandName { get; set; }
            public string CategoryName { get; set; }
        }

        public static List<DetailedProduct> ConvertDataTableToGenericList(string connectionString)
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
                if (dt.Rows.Count > 0)
                {
                    var products =
                        from row in dt.AsEnumerable()  // must convert to an enumerable object
                        select new DetailedProduct
                        {
                            BrandName = row.Field<string>("brand_name"),
                            CategoryName = row.Field<string>("category_name"),
                            ListPrice = row.Field<decimal>("list_price"),
                            ModelYear = row.Field<short>("model_year"),
                            ProductName = row.Field<string>("product_name")
                        };
                    return products.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
