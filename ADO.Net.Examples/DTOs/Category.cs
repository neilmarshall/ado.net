using ADO.Net.Examples.Wrappers;

namespace ADO.Net.Examples.DTOs
{
    public class Category
    {
        [DataColumn("category_id")]
        public int Id { get; set; }
        [DataColumn("category_name")]
        public string Name { get; set; }
    }
}
