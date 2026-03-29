using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<WilliamMillerSite.Services.Resume.IResumeContentSource, WilliamMillerSite.Services.Resume.JsonResumeContentSource>();
builder.Services.AddScoped<WilliamMillerSite.Services.ResumeService>();

var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
if (isDocker)
{
    var dataProtectionKeysPath = "/root/.aspnet/DataProtection-Keys";
    if (Directory.Exists(dataProtectionKeysPath))
    {
        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath));
    }
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!isDocker)
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute("downloadResume", "Resume/Download", new { controller = "Home", action = "DownloadResume" });
app.MapControllerRoute("resume", "Resume", new { controller = "Home", action = "Resume" });
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

if (isDocker)
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
    app.Run($"http://0.0.0.0:{port}");
}
else
{
    app.Run();
}
