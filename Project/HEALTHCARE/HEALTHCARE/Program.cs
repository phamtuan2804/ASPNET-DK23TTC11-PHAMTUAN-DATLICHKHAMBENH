using Microsoft.EntityFrameworkCore;
using HEALTHCARE.Data; // Namespace cho DbContext và Entities
using HEALTHCARE.Models; // Namespace cho Models (nếu cần)

var builder = WebApplication.CreateBuilder(args);

// Lấy chuỗi kết nối
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Chuỗi kết nối 'DefaultConnection' không được tìm thấy trong appsettings.json.");
}

// Đăng ký DbContext
builder.Services.AddDbContext<HealthCareDbContext>(options =>
    options.UseSqlServer(connectionString));

// Đăng ký các dịch vụ khác
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình pipeline xử lý HTTP request
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Sử dụng các middleware cần thiết
app.UseHttpsRedirection();
app.UseStaticFiles(); // Sử dụng file tĩnh (CSS, JS, Images)
app.UseRouting();
app.UseAuthorization();

// Cấu hình route mặc định trỏ đến trang đặt lịch
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DatLich}/{action=TaoMoi}/{id?}");

app.Run();