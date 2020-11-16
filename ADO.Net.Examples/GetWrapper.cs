using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ADO.Net.Examples.Wrappers
{
    class DataColumnAttribute : Attribute
    {
        public string ColumnName { get; }

        public DataColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }

    class GetWrapper
    {
        private readonly string _connectionString;

        public GetWrapper(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public List<T> GetRecords<T>(string query)
        {
            using var conn = new SqlConnection(this._connectionString);
            using var cmd = new SqlCommand(query, conn);
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            return dt.AsEnumerable().Select(row =>
            {
                var item = Activator.CreateInstance<T>();
                foreach (var propertyInfo in typeof(T).GetProperties())
                {
                    foreach (var cad in propertyInfo.GetCustomAttributesData())
                    {
                        if (cad.AttributeType == typeof(DataColumnAttribute))
                        {
                            var columnName = (string)cad.ConstructorArguments[0].Value;
                            propertyInfo.SetValue(item, row[columnName]);
                        }
                    }
                }

                return item;
            }).ToList();
        }
    }
}
