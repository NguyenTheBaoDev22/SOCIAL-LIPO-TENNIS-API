using API.BackgroundServices;
using API.Conventions;
using API.Extensions;
using API.Middlewares;
using Applications;
using Applications.Behaviors;
using Applications.Features.ClientCredentials.Commands;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.MappingProfiles;
using Applications.Services.Implementations;
using Applications.Services.Interfaces;
using AspNetCoreRateLimit;
using Core.Interfaces;
using FluentValidation;
using Infrastructure.Caching;
using Infrastructure.DependencyInjections;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Infrastructure.Persistences;
using Infrastructure.Persistences.Interceptors;
using Infrastructure.Persistences.Repositories;
using Infrastructure.Redis;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Shared.Configs;
using Shared.Constants;
using Shared.Filters;
using Shared.Interfaces;
using Shared.Interfaces.Shared.Interfaces;
using Shared.Middleware;
using Shared.Options;
using Shared.Services;
using System.Text;
using System.Text.Json;
using IpRateLimitOptions = AspNetCoreRateLimit.IpRateLimitOptions;



Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
    .WriteTo.File(new Serilog.Formatting.Json.JsonFormatter(), "logs/zenshop-log.json", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
        .WriteTo.File("logs/ZenShop.log", rollingInterval: RollingInterval.Day);
});

builder.Services.Configure<VietQrOptions>(builder.Configuration.GetSection(VietQrOptions.SectionName));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<AuditSaveChangesInterceptor>();
builder.Services.AddScoped<AuditLogInterceptor>();

builder.Services.AddDbContext<AppDbContext>((provider, options) =>
{
    options.UseNpgsql(connectionString);
    options.AddInterceptors(
        provider.GetRequiredService<AuditSaveChangesInterceptor>(),
        provider.GetRequiredService<AuditLogInterceptor>()); // thứ tự không ảnh hưởng vì độc lập
});
builder.Services.AddInfrastructureServices(connectionString);
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.Configure<S3Setting>(builder.Configuration.GetSection("S3Setting"));
builder.Services.AddSingleton<IStorageService, S3StorageService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddSingleton<IAppSettings>(sp =>
    sp.GetRequiredService<IOptions<AppSettings>>().Value);
builder.Services.Configure<ZaloConfig>(builder.Configuration.GetSection("ZaloMiniAppConfig"));
builder.Services.Configure<LarkSuiteConfig>(builder.Configuration.GetSection("LarkSuiteConfig"));
builder.Services.AddSingleton<ILarkSuiteConfig>(sp =>
    sp.GetRequiredService<IOptions<LarkSuiteConfig>>().Value);
// Đăng ký LarkAuthService
builder.Services.AddScoped<ILarkAuthService, LarkAuthService>();
builder.Services.AddHttpClient<ILarkTokenService, LarkTokenService>();
builder.Services.AddHttpClient<ILarkEmailService, LarkEmailService>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ApplicationAssembly).Assembly);
});

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


// External Configuration & Services
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<JwtAppUserOptions>(
    builder.Configuration.GetSection("JwtSettings:AppUser"));
builder.Services.Configure<JwtServiceOptions>(
    builder.Configuration.GetSection("JwtSettings:Service"));

// Đăng ký memory cache để lưu counter, policy
builder.Services.AddMemoryCache();
//builder.Services.AddMemoryCache(options =>
//{
//    options.SizeLimit = 512; // tổng số item hoặc MB, tùy co
//});
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
// Đọc cấu hình IpRateLimiting từ appsettings.json
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
// Sử dụng store lưu chính sách IP và counter trong bộ nhớ (in-memory)
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
// Register the processing strategy to avoid error
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
// Cấu hình dịch vụ rate limit
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

//builder.Services.AddSmartRedisCache(builder.Configuration);

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAppUserJwtTokenGenerator, AppUserJwtTokenGenerator>();
builder.Services.AddSingleton<JwtTokenValidator>();
builder.Services.AddScoped<IPaginationHelper, PaginationHelper>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IClientCredentialRepository, ClientCredentialRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<ISpamProtectionService, RedisSpamProtectionService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddValidatorsFromAssembly(typeof(CreateClientCredentialCommand).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.Configure<MBFSMSConfig>(builder.Configuration.GetSection("MBFSMSConfig"));
builder.Services.AddHttpClient<IMBFSmsBranchnameService, MBFSmsBranchnameService>();
builder.Services.AddScoped<ISmsLogRepository, SmsLogRepository>();
builder.Services.AddScoped<ISmsRetryQueueRepository, SmsRetryQueueRepository>();

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    options.Filters.Add<ValidateModelAttribute>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// =============================
// 🔐 JWT Authentication
// =============================

//builder.Services.AddJwtAuthentication(builder.Configuration);
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = "ZenShopAPI",
        ValidAudience = "ZenShopClient",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.AppUser.Secret))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewAuditLogOrAdmin", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.HasClaim("permission", "CanViewAuditLog")));
});
// =============================
// 🧪 Swagger
// =============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Nhập token dạng: **Bearer your-token-here**"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
//builder.Services.AddHostedService<LarkTokenCacheWarmupService>();



var app = builder.Build();

// ✅ Middleware pipeline
app.UseMiddleware<TraceIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<JsonExceptionMiddleware>();
app.UseMiddleware<ModelBindingValidationMiddleware>();
app.UseMiddleware<ValidationExceptionHandlerMiddleware>();
app.UseMiddleware<RequestEnrichmentMiddleware>();
await app.MigrateAndSeedAsync();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Localhost"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZenShop API V1");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseIpRateLimiting(); // Kích hoạt IP rate limiting middleware
app.UseAuthentication();
app.UseAuthorization();

// ✅ Logging, permission middleware trước controllers
app.UseMiddleware<SerilogEnrichMiddleware>();
app.UseMiddleware<AuthorizeByPermissionMiddleware>();

app.MapControllers();
app.Run();
