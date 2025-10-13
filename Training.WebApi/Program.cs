using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Serilog;
using Training.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ====================================================
// 1️⃣ Serilog cấu hình logging
// ====================================================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// ====================================================
// 2️⃣ i18n (Localization)
// ====================================================
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Thêm localization cho Controllers (nếu sau này dùng View hoặc DataAnnotation)
builder.Services.AddControllers()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Cấu hình các ngôn ngữ hỗ trợ
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("ja"),
    new CultureInfo("vi")
};

// ====================================================
// 3️⃣ Add các service khác (Infrastructure, OpenAPI...)
// ====================================================
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

// ====================================================
// 4️⃣ Build app
// ====================================================
var app = builder.Build();

// Cấu hình RequestLocalization
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// ====================================================
// 5️⃣ Configure HTTP request pipeline
// ====================================================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ====================================================
// 6️⃣ Khởi động ứng dụng + log startup error
// ====================================================
try
{
    Log.Information("Starting up the application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed!");
}
finally
{
    Log.CloseAndFlush();
}
