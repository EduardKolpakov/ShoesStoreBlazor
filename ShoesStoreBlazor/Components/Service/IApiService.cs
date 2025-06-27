using ShoesStoreBlazor.Components.Service.Classes;

namespace ShoesStoreBlazor.Components.Service
{
    public interface IApiService
    {
        public Task<string> Authorization(string login, string password);
        public Task<List<ProductDto>> GetProductsAsync();
        public Task<List<CategoryDto>> GetCategoriesAsync();
        public Task<bool> AddToCart(CartItemRequest newCart);
        public void SetToken(string token);
        public Task<List<CartItemDto>> GetCartAsync();
        public Task<bool> RemoveFromCartAsync(int itemId);
        public Task<bool> ClearCartAsync();
        public Task<ProductDto> GetProductByID(int id);
        public Task<bool> UpdateCartItem(CartItemDto newCart);
        public Task<UserDto> GetCurrentUserAsync();
    }
}
