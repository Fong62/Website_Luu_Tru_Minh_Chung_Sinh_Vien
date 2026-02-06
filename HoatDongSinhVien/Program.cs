using HoatDongSinhVien.Data;
using HoatDongSinhVien.Models.Application_User;
using HoatDongSinhVien.Models.Data;
using HoatDongSinhVien.Models.Services;
using HoatDongSinhVien.Models.Services_Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<HoatDongSinhVienDbcontext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultString"));
});

ExcelPackage.License.SetNonCommercialPersonal("Student Project");

//DI


builder.Services.AddSession();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
    options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 1;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.User.RequireUniqueEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.SignIn.RequireConfirmedEmail = false;
    }
    )
    .AddEntityFrameworkStores<HoatDongSinhVienDbcontext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ITaoUpdateGGForm, TaoUpdateGGForm>();
builder.Services.AddHttpClient<InterfaceOCR, OCRRepository>();
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await SeedData.InitializeAsync(services);
    await SeedUser.InitializeAsync(services);

}

app.Run();
