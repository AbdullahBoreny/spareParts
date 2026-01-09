using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spareParts.Services
{
    public class ApiService
    {
        public HttpClient _httpClient;
        public string _baseUrl;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _baseUrl = "http://20.64.249.9/SpareParts/api/"; // TODO: Configure from app settings
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<T>(json) ?? throw new InvalidOperationException("Deserialization returned null");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<T>(responseJson) ?? throw new InvalidOperationException("Deserialization returned null");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task LogoutAsync()
        {
            // TODO: Implement logout logic
            await Task.Delay(500);
        }
    }
}
