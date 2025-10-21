using Training.Application;


var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// 2️⃣ Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("ja"),
    new CultureInfo("vi")
};

// 3️⃣ Add Infrastructure + Application
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// 4️⃣ FluentValidation + Controllers + Custom lỗi
builder.Services.AddControllers(options =>
    {
        options.Filters.Add<CustomValidatorInterceptor>();
    })
    .AddViewLocalization()
    .AddDataAnnotationsLocalization()
    .AddFluentValidation(fv =>
    {
        // ✅ Đăng ký validator
        fv.RegisterValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>();
        fv.DisableDataAnnotationsValidation = true;
    })
    .AddCustomValidationResponse();

// Đăng ký validator thủ công (nếu cần)
builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();

// 5️⃣ JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
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
builder.Services.AddAutoMapper(typeof(ApplicationProfile));

// 6️⃣ Build app
var app = builder.Build();

// RequestLocalization
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// 7️⃣ Pipeline
if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtTokenFilter>();
app.MapControllers();

// 8️⃣ Run
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
