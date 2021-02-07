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
        public void GetEmployeesByStoreMethod1Fixture()
        {
            var store = dapperDemoRepository.GetEmployeesByStoreMethod1("Baldwin");

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
        public void GetEmployeesByStoreMethod2Fixture()
        {
            var store = dapperDemoRepository.GetEmployeesByStoreMethod2("Baldwin");

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

    }
}
