using System.Collections.Generic;

namespace DapperDemo
{
    public class Store
    {
        public int Id { get; set; }
        public string City { get; set; }
        public List<Employee> Employees { get; set; }
    }
}