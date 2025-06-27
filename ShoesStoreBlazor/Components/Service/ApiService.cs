using Microsoft.JSInterop;
using ShoesStoreBlazor.Components.Service.Classes;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoesStoreBlazor.Components.Service
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        public string? AuthToken { get; set; }

        public ApiService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        private async Task ApplyAuthorizationHeaderAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            string? token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }


        public void SetToken(string token)
        { 
            AuthToken = token;
        }

        public async Task<string> Authorization(string login, string password)
        {
            LoginRequest lr = new LoginRequest();
            lr.Login = login;
            lr.Password = password;
            var request = new HttpRequestMessage(HttpMethod.Post, "api/User/login")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(lr),
                    Encoding.UTF8,
                    "application/json")
            };
            try
            {
                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AuthResponse>(json);

                return result?.token ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Здесь можно логировать или пробрасывать исключение
                Console.WriteLine($"Ошибка при авторизации: {ex}");
                throw;
            }
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var request = await _httpClient.GetAsync("api/shoes/getallshoes");
            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GetProductsResponse>(response);
                return result.shoes;
            }
            else return null;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var request = await _httpClient.GetAsync("api/shoes/getallcategories");
            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GetCategoriesResponse>(response);
                return result.categories;
            }
            else return null;
        }

        public async Task<bool> AddToCart(CartItemRequest newCart)
        {
            await ApplyAuthorizationHeaderAsync();
            var request = await _httpClient.PostAsJsonAsync<CartItemRequest>("api/cart/add", newCart);
            request.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<List<CartItemDto>> GetCartAsync()
        {
            await ApplyAuthorizationHeaderAsync();
            var request = await _httpClient.GetAsync("api/cart/get");
            if (request.IsSuccessStatusCode)
            { 
                var response = await request.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GetCartResponse>(response);
                return result.items;
            }
            return null;
        }

        public async Task<bool> RemoveFromCartAsync(int itemId)
        {
            await ApplyAuthorizationHeaderAsync();
            var request = await _httpClient.DeleteAsync($"api/cart/delete/{itemId}");
            request.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<bool> ClearCartAsync()
        {
            await ApplyAuthorizationHeaderAsync();
            var request = await _httpClient.DeleteAsync($"api/cart/clear");
            request.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<ProductDto> GetProductByID(int id)
        {
            var request = await _httpClient.GetAsync($"api/shoes/getshoes/{id}");
            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ProductDto>(response);
                return result;
            }
            return null;
        }

        public async Task<bool> UpdateCartItem(CartItemDto newCart)
        {
            await ApplyAuthorizationHeaderAsync();
            var request = await _httpClient.PutAsJsonAsync<CartItemDto>($"api/cart/Update/{newCart.id}", newCart);
            request.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            await ApplyAuthorizationHeaderAsync();
            var request = await _httpClient.GetAsync("api/user/me");
            if (request.IsSuccessStatusCode)
            { 
                var response = await request.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<UserDto>(response);
                return result;
            }
            return null;
        }

        public class GetCartResponse
        { 
            public List<CartItemDto> items { get; set; }
        }

        public class AuthResponse
        {
            public string token { get; set; }
        }

        public class GetProductsResponse
        { 
            public List<ProductDto> shoes { get; set; }
        }

        public class GetCategoriesResponse
        { 
            public List<CategoryDto> categories { get; set; }
        }

    }
}
