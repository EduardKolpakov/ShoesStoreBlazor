namespace ShoesStoreBlazor.Components.Service.Classes
{
    public class ProductDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public int categoryId { get; set; }
        public decimal price { get; set; }
        public string sizes { get; set; }
        public string description { get; set; }
        public string imageUrl { get; set; }
    }

}
