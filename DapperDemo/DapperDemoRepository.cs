using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DapperDemo
{
    public class DapperDemoRepository
    {
        private readonly SqlConnection db;

        public DapperDemoRepository(string connString)
        {
            this.db = new SqlConnection(connString);
        }

        public Store GetEmployeesByStoreMethod1(string city)
        {
            var store = this.db.Query<Store>(
                    "SELECT store_id id, city FROM sales.stores WHERE city = @city",
                    new { city })
                .Single();

            var employees = this.db.Query<Employee>(
                    "SELECT first_name FirstName, last_name LastName FROM sales.staffs WHERE store_id = @id",
                    store)
                .ToList();

            if (store != null && employees != null)
            {
                store.Employees = employees;
            }

            return store;
        }

        public Store GetEmployeesByStoreMethod2(string city)
        {
            var gridReader = this.db.QueryMultiple(
                    "SELECT store_id id, city FROM sales.stores WHERE city = @city;" +
                    "SELECT first_name FirstName, last_name LastName FROM sales.staffs WHERE store_id = (SELECT store_id id FROM sales.stores WHERE city = @city);",
                    new { city });

            var store = gridReader.Read<Store>().Single();
            var employees = gridReader.Read<Employee>().ToList();

            if (store != null && employees != null)
            {
                store.Employees = employees;
            }

            return store;
        }

        public List<Employee> GetEmployeesWithStore()
        {
            var employees = this.db.Query<Employee, Store, Employee>(
                    @"SELECT first_name FirstName, last_name LastName, city
                        FROM sales.stores
                        JOIN sales.staffs
                          ON sales.stores.store_id = sales.staffs.store_id;",
                    (employee, store) =>
                    {
                        employee.Store = store;
                        return employee;
                    },
                    splitOn: "city").ToList();

            return employees;
        }
    }
}
