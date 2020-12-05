using System;
using System.Data;
using System.Data.SqlClient;

namespace ADO.Net.Examples
{
    public static partial class DBFunctions
    {
        public static void GetMultipleResultSetsAsDataSet(string connectionString)
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
