using API.IRepository;
using API.IRepository.Repository;
using API.Repository;
using Data.IRepository;
using Data.Models;
using Data.Repositories;
using Data.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

// =====================================================
// DATABASE
// =====================================================

builder.Services.AddDbContext<DbContextApp>(
    options =>
    {
        options.UseSqlServer(
            builder.Configuration
                .GetConnectionString(
                    "DefaultConnection"));
    });

// =====================================================
// SWAGGER & CONTROLLERS
// =====================================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });


// =====================================================
// ĐĂNG KÝ REPOSITORY
// =====================================================

builder.Services.AddScoped<
    IDiaChiKhachHangRepository,
    DiaChiKhachHangRepository>();
builder.Services.AddScoped<
    INhanVienRepository,
    NhanVienRepository>();
builder.Services.AddScoped<
    ITaiKhoanRepository,
    TaiKhoanRepository>();
builder.Services.AddScoped<
    IChucVuRepository,
    ChucVuRepository>();
builder.Services.AddScoped<
    IThongBaoRepository,
    ThongBaoRepository>();
builder.Services.AddScoped<
    IKhachHangRepository,
    KhachHangRepository>();

var app = builder.Build();

// Cấu hình môi trường HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();