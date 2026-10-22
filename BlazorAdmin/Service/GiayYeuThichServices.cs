
using API.Models;
using BlazorAdmin.IServices;
using System.Net.Http.Json;

namespace BlazorAdmin.Services
{
    public class GiayYeuThichService : IGiayYeuThichService
    {
        private readonly HttpClient _http;
        private const string ApiUrl = "api/GiayYeuThich";

        public GiayYeuThichService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<GiayYeuThich>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<GiayYeuThich>>(ApiUrl)
               ?? new List<GiayYeuThich>();

        public async Task<GiayYeuThich?> GetByIdAsync(Guid id)
        {
            var response = await _http.GetAsync($"{ApiUrl}/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<GiayYeuThich>();
        }

        public async Task<bool> CreateAsync(GiayYeuThich item)
            => (await _http.PostAsJsonAsync(ApiUrl, item)).IsSuccessStatusCode;

        public async Task<bool> UpdateAsync(Guid id, GiayYeuThich item)
            => (await _http.PutAsJsonAsync($"{ApiUrl}/{id}", item)).IsSuccessStatusCode;

        public async Task<bool> DeleteAsync(Guid id)
            => (await _http.DeleteAsync($"{ApiUrl}/{id}")).IsSuccessStatusCode;
    }
}
