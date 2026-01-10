using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace spareParts.Services
{
    public class ApiService
    {
        public HttpClient _httpClient;
        public string _baseUrl;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _baseUrl = "http://20.64.249.9/SpareParts/api/sync"; // TODO: Configure from app settings
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseString) ?? throw new InvalidOperationException("Deserialization returned null");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> PostAsync(string endpoint, object data)
        {
            var json = JsonSerializer.Serialize(data);
            
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);

            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(responseString);

            return responseString;
        }

        public async Task LogoutAsync()
        {
            // TODO: Implement logout logic
            await Task.Delay(500);
        }
        
    }
}
