
using BlazorAdmin.IServices;
using BlazorAdmin.Service.IService;
using Data.Models;
using System.Net.Http.Json;

namespace BlazorAdmin.Services
{
    public class DeGiayService : IDeGiayService
    {
        private readonly HttpClient _http;
        private const string ApiUrl = "api/DeGiay";

        public DeGiayService(HttpClient http) => _http = http;

        public async Task<List<DeGiay>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<DeGiay>>(ApiUrl)
               ?? new List<DeGiay>();

        public async Task<DeGiay?> GetByIdAsync(Guid id)
        {
            var res = await _http.GetAsync($"{ApiUrl}/{id}");
            return res.IsSuccessStatusCode
                ? await res.Content.ReadFromJsonAsync<DeGiay>()
                : null;
        }

        public async Task<bool> CreateAsync(DeGiay item)
            => (await _http.PostAsJsonAsync(ApiUrl, item)).IsSuccessStatusCode;

        public async Task<bool> UpdateAsync(Guid id, DeGiay item)
            => (await _http.PutAsJsonAsync($"{ApiUrl}/{id}", item)).IsSuccessStatusCode;

        public async Task<bool> DeleteAsync(Guid id)
            => (await _http.DeleteAsync($"{ApiUrl}/{id}")).IsSuccessStatusCode;
    }
}