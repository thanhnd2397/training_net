using System.Globalization;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Training.Application.Dtos.Request;
using Training.Infrastructure;
using Training.WebApi.Filter;
using Training.WebApi.Extension;
using Training.WebApi.Validations;

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

var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("ja"),
    new CultureInfo("vi")
};

// ====================================================
// 3️⃣ Add Infrastructure + JWT Authentication
// ====================================================

// 🟢 GỌI HÀM NÀY ĐỂ ĐĂNG KÝ CÁC SERVICE NHƯ IMessageService, ILoginUseCase,...
builder.Services.AddInfrastructure(builder.Configuration);

// ⚙️ FluentValidation + Controllers + Custom lỗi
builder.Services.AddControllers()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>();
        fv.DisableDataAnnotationsValidation = true;
    })
    .AddCustomValidationResponse();

// Đăng ký thủ công validator cho chắc chắn
builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();

// 🔐 Cấu hình JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // chỉ nên false khi dev
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

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

// 1️⃣ Exception handler nên bao ngoài toàn bộ pipeline
app.UseMiddleware<GlobalExceptionMiddleware>();

// 2️⃣ Authentication & Authorization nên trước các custom filter/token middleware
app.UseAuthentication();
app.UseAuthorization();

// 3️⃣ Các middleware custom xử lý request (như JwtTokenFilter)
app.UseMiddleware<JwtTokenFilter>();

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
