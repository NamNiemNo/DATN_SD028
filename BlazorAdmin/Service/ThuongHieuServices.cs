
using BlazorAdmin.IServices;
using Data.Models;
using System.Net.Http.Json;

namespace BlazorAdmin.Services
{
    public class ThuongHieuService : IThuongHieuService
    {
        private readonly HttpClient _http;
        private const string ApiUrl = "api/ThuongHieu";

        public ThuongHieuService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ThuongHieu>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<ThuongHieu>>(ApiUrl)
               ?? new List<ThuongHieu>();

        public async Task<ThuongHieu?> GetByIdAsync(Guid id)
        {
            var response = await _http.GetAsync($"{ApiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<ThuongHieu>();
        }

        public async Task<bool> CreateAsync(ThuongHieu item)
            => (await _http.PostAsJsonAsync(ApiUrl, item)).IsSuccessStatusCode;

        public async Task<bool> UpdateAsync(Guid id, ThuongHieu item)
            => (await _http.PutAsJsonAsync($"{ApiUrl}/{id}", item))
                .IsSuccessStatusCode;

        public async Task<bool> DeleteAsync(Guid id)
            => (await _http.DeleteAsync($"{ApiUrl}/{id}")).IsSuccessStatusCode;
    }
}
