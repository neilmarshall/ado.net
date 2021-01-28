using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ADO.Net.Examples.DTOs;
using ADO.Net.Examples.Wrappers;
using static ADO.Net.Examples.DBFunctions;

namespace ADO.Net.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("Config.json").Build();
            var connectionString = configuration.GetConnectionString("AzureDBConnection");

            ExecuteScalar(connectionString);
            await Task.WhenAll(
                ExecuteScalarAsync(connectionString),
                ExecuteStoredProcedureWithOutputParameterAsync(connectionString),
                ExecuteStoredProcedureReturningTable(connectionString));
            ExecuteReader(connectionString);
            ExecuteReaderWithTypeConversionAndNullChecking(connectionString);
            ExecuteReaderWithMultipleResultSets(connectionString);
            GetQueryAsDataTable(connectionString);
            Console.WriteLine(ConvertDataTableToGenericList(connectionString).Select(dp => dp.ListPrice).Sum());
            GetMultipleResultSetsAsDataSet(connectionString);
            ExecuteStoredProcedureWithTableValueParameter(connectionString);

            var brands = (new GetWrapper(connectionString)).GetRecords<Brand>("SELECT * FROM [production].[brands];");
            var categories= (new GetWrapper(connectionString)).GetRecords<Category>("SELECT * FROM [production].[categories];");
        }
    }
}
