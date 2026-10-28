
using BlazorAdmin.IServices;
using BlazorAdmin.Service.IService;
using Data.Models;
using System.Net.Http.Json;

namespace BlazorAdmin.Services
{
    public class TheLoaiGiayService : ITheLoaiGiayService
    {
        private readonly HttpClient _http;
        private const string ApiUrl = "api/TheLoaiGiay";

        public TheLoaiGiayService(HttpClient http) => _http = http;

        public async Task<List<TheLoaiGiay>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<TheLoaiGiay>>(ApiUrl)
               ?? new List<TheLoaiGiay>();

        public async Task<TheLoaiGiay?> GetByIdAsync(Guid id)
        {
            var res = await _http.GetAsync($"{ApiUrl}/{id}");
            return res.IsSuccessStatusCode
                ? await res.Content.ReadFromJsonAsync<TheLoaiGiay>()
                : null;
        }

        public async Task<bool> CreateAsync(TheLoaiGiay item)
            => (await _http.PostAsJsonAsync(ApiUrl, item)).IsSuccessStatusCode;

        public async Task<bool> UpdateAsync(Guid id, TheLoaiGiay item)
            => (await _http.PutAsJsonAsync($"{ApiUrl}/{id}", item)).IsSuccessStatusCode;

        public async Task<bool> DeleteAsync(Guid id)
            => (await _http.DeleteAsync($"{ApiUrl}/{id}")).IsSuccessStatusCode;
    }
}