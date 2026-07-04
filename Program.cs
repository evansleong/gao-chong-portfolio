using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using GaoChongPortfolio.Services;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

// Register Portfolio Repository Service (Singleton is perfect for file-based JSON database)
builder.Services.AddSingleton<IPortfolioService, PortfolioService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// 1. Serve default wwwroot assets (JS, CSS, static images)
app.UseStaticFiles();

// 2. Resolve a persistent writeable directory for uploads, especially for Azure Run-From-Package
string uploadsPath;
if (Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID") != null)
{
    // Azure persistent writeable volume
    uploadsPath = "/home/site/uploads";
}
else
{
    // Local development fallback
    uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "images", "uploads");
}

if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

// 3. Serve uploaded files from this writeable directory under the "/images/uploads" path
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/images/uploads"
});

// Re-execute request on status codes like 404 for our funny 404 page
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

app.UseRouting();

// Authentication MUST run before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
