using Microsoft.EntityFrameworkCore;
using HEALTHCARE.Data;
using HEALTHCARE.Models; 

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Chuỗi kết nối 'DefaultConnection' không được tìm thấy trong appsettings.json.");
}

builder.Services.AddDbContext<HealthCareDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DatLich}/{action=TaoMoi}/{id?}");

app.Run();