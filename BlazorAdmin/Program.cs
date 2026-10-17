using BlazorAdmin.Components;
using BlazorAdmin.Service.IService;
using BlazorAdmin.Service;

var builder = WebApplication.CreateBuilder(args);

// 1. Thêm dịch vụ Razor Components (Blazor)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Thêm Controllers và cấu hình JSON (chống lỗi vòng lặp tham chiếu)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// 3. Cấu hình HttpClient cơ bản để gọi API (Thay đổi BaseAddress thành URL của API của bạn nếu cần)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7246/")
});
// khai báo services
builder.Services.AddScoped<INhanVienService, NhanVienService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Ánh xạ các Controller API nếu bạn viết API chung trong dự án này
app.MapControllers();

app.Run();