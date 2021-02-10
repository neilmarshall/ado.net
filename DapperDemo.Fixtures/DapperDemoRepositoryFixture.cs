using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DapperDemo.Fixtures
{
    [TestClass]
    public class DapperDemoRepositoryFixture
    {
        private static DapperDemoRepository dapperDemoRepository;

        [ClassInitialize]
        public static void InitializeClass(TestContext _)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            dapperDemoRepository = new DapperDemoRepository(configuration.GetConnectionString("DefaultConnectionString"));
        }

        [TestMethod]
        public void GetEmployeesByStoreMultipleQueriesFixture()
        {
            var store = dapperDemoRepository.GetEmployeesByStoreMultipleQueries("Baldwin");

            Assert.AreEqual("Baldwin", store.City);
            CollectionAssert.AreEqual(
                new List<Employee>
                {
                    new Employee { FirstName = "Jannette", LastName = "David" },
                    new Employee { FirstName = "Marcelene", LastName = "Boyer" },
                    new Employee { FirstName = "Venita", LastName = "Daniel" }
                },
                store.Employees);
        }

        [TestMethod]
        public void GetEmployeesByStoreQueryMultipleFixture()
        {
            var store = dapperDemoRepository.GetEmployeesByStoreQueryMultiple("Baldwin");

            Assert.AreEqual("Baldwin", store.City);
            CollectionAssert.AreEqual(
                new List<Employee>
                {
                    new Employee { FirstName = "Jannette", LastName = "David" },
                    new Employee { FirstName = "Marcelene", LastName = "Boyer" },
                    new Employee { FirstName = "Venita", LastName = "Daniel" }
                },
                store.Employees);
        }

        [TestMethod]
        public void GetEmployeesWithStoreFixture()
        {
            var employees = dapperDemoRepository.GetEmployeesWithStore();

            Assert.AreEqual(10, employees.Count);
            Assert.AreEqual("Fabiola", employees[0].FirstName);
            Assert.AreEqual("Jackson", employees[0].LastName);
            Assert.AreEqual("Santa Cruz", employees[0].Store.City);
        }

        [TestMethod]
        public void DemonstrateListSupportForInOperator()
        {
            var employees = dapperDemoRepository.GetEmployeesWithIds(new[] { 4, 6, 7 });

            CollectionAssert.AreEqual(
                new[] { "marcelene.boyer@bikes.shop", "venita.daniel@bikes.shop", "virgie.wiggins@bikes.shop" },
                employees);
        }

        [TestMethod]
        public void DemonstrateStoredProcedureExecution()
        {
            Assert.AreEqual(3, dapperDemoRepository.GetStoreCount());
        }
    }
}
