using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using CampaignManagement.Helpers;
using CampaignManagement.Helpers.DbContexts;
using CampaignManagement.Helpers.Middlewares;
using CampaignManagement.Interfaces;
using CampaignManagement.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Fix: ASP.NET Core auto-registers a Windows EventLog logging provider at Warning level
// on Windows by default. If the process account can't write to the Windows Event Log,
// the very first warning-level log call (e.g. from HttpsRedirectionMiddleware) throws
// an AggregateException and crashes the request pipeline. Clear providers and use
// Console/Debug only, which always work regardless of OS permissions.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllersWithViews();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 524288000; // 500 MB
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 524288000; // 500MB
});

builder.Services.AddControllers();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
//builder.Services.AddDbContext<CampaignDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnectionString"), new MySqlServerVersion(new Version(8, 0, 36))));
var cs = builder.Configuration.GetConnectionString("MySQLConnectionString");

Console.WriteLine("================================");
Console.WriteLine(cs);
Console.WriteLine("================================");

builder.Services.AddDbContext<CampaignDbContext>(options =>
    options.UseMySql(
        cs,
        new MySqlServerVersion(new Version(8, 0, 36))));

// Register Repositories
builder.Services.AddScoped<IApiResponseRepository, ApiResponseRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICommonRepository, CommonRepository>();
builder.Services.AddScoped<IPermissionHelper, PermissionHelper>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<ICampaignsRepository, CampaignsRepository>();
builder.Services.AddScoped<IInfluencersStatRepository, InfluencersStatRepository>();
builder.Services.AddScoped<IReportsRepository, ReportsRepository>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<ICreatorCodesRepository, CreatorCodesRepository>();

// Register Middlewares
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddTransient<ActivityLoggingMiddleware>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddDistributedMemoryCache();
var dataProtectionKeysPath = Path.Combine(builder.Environment.ContentRootPath, "DataProtection-Keys");
Directory.CreateDirectory(dataProtectionKeysPath);
builder.Services.AddDataProtection()
    .SetApplicationName("CampaignManagement")
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseStaticFiles();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<ActivityLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(name: "default", pattern: "{controller=Auth}/{action=Login}/{id?}").WithStaticAssets();
app.Run();
