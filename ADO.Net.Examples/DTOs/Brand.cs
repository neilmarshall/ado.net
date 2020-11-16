using ADO.Net.Examples.Wrappers;

namespace ADO.Net.Examples.DTOs
{
    public class Brand
    {
        [DataColumn("brand_id")]
        public int Id { get; set; }
        [DataColumn("brand_name")]
        public string Name { get; set; }
    }
}
