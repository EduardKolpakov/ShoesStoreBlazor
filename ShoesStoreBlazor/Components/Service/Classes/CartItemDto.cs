namespace ShoesStoreBlazor.Components.Service.Classes
{
    public class CartItemDto
    {
        public int id { get; set; }
        public int userId { get; set; }
        public UserDto user { get; set; }
        public int shoesId { get; set; }
        public ProductDto shoes {  get; set; }
        public string size { get; set; }
        public int quantity { get; set; }
    }
}
