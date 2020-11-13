using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ADO.Net.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AzureDBConnection"].ConnectionString;
            ExecuteScalar(connectionString);
            await Task.WhenAll(
                ExecuteScalarAsync(connectionString),
                ExecuteStoredProcedureWithOutputParameterAsync(connectionString));
            ExecuteReader(connectionString);
            ExecuteReaderWithTypeConversionAndNullChecking(connectionString);
            ExecuteReaderWithMultipleResultSets(connectionString);
            GetQueryAsDataTable(connectionString);
            Console.WriteLine(ConvertDataTableToGenericList(connectionString).Select(dp => dp.ListPrice).Sum());
            GetMultipleResultSetsAsDataSet(connectionString);
        }

        /// <summary>
        /// The 'ExecuteScalar' method executes the query, and returns the first
        /// column of the first row in the result set returned by the query (additional
        /// columns or rows are ignored
        /// </summary>
        static void ExecuteScalar(string connectionString)
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

        /// <summary>
        /// The 'ExecuteScalarAsync' method executes the query asynchronously, and returns the first
        /// column of the first row in the result set returned by the query (additional
        /// columns or rows are ignored
        /// </summary>
        static async Task ExecuteScalarAsync(string connectionString)
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

        /// <summary>
        /// The 'ExecuteNonQueryAsync' method can be used to execute a stored procedure with
        /// an output parameter asynchronously
        /// </summary>
        static async Task ExecuteStoredProcedureWithOutputParameterAsync(string connectionString)
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

        /// <summary>
        /// The 'ExecuteReader' method creates a data reader (a forward-only cursor) that
        /// can be used to iterate through one or more result sets
        /// </summary>
        static void ExecuteReader(string connectionString)
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

        /// <summary>
        /// The 'IsDBNull' method on the DataReader class checks if a value is
        /// null - this is useful before attempting to cast to a non-nullable type
        /// </summary>
        static void ExecuteReaderWithTypeConversionAndNullChecking(string connectionString)
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

        /// <summary>
        /// The 'NextResult' method on the DataReader class is used to iterate through
        /// subsequent result sets
        /// </summary>
        static void ExecuteReaderWithMultipleResultSets(string connectionString)
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

        static void GetQueryAsDataTable(string connectionString)
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

        private class DetailedProduct
        {
            public string ProductName { get; set; }
            public int ModelYear { get; set; }
            public decimal ListPrice { get; set; }
            public string BrandName { get; set; }
            public string CategoryName { get; set; }
        }

        static List<DetailedProduct> ConvertDataTableToGenericList(string connectionString)
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

        static void GetMultipleResultSetsAsDataSet(string connectionString)
        {
            try
            {
                DataSet ds = new DataSet();
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("SELECT store_name FROM sales.stores; SELECT manager_id FROM sales.staffs;", conn);
                using var da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    var storeNames = ds.Tables[0];
                    foreach (var storeName in storeNames.AsEnumerable())
                    {
                        Console.WriteLine($"\tStore: {storeName.Field<string>("store_name")}");
                    }

                    var managerIds = ds.Tables[1];
                    foreach (var managerId in managerIds.AsEnumerable())
                    {
                        Console.WriteLine($"\tManager ID: {managerId.Field<int?>("manager_id") ?? 0}");
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
