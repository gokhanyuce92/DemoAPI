using System.Text;
using Demo.Entities;
using Demo.Interfaces;
using Demo.Repositories.Abstract;
using Demo.Repositories.Concrete;
using Demo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
var redisConnection = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
var MySQLConnection = configuration.GetConnectionString("MySQLConnection");

// Serilog yapılandırması
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.MySQL(
        connectionString: MySQLConnection,
        tableName: "Logs"
    )
    // .Enrich.With(new UserNameEnricher(builder.Services.BuildServiceProvider().GetService<IHttpContextAccessor>()))
    .CreateLogger();
builder.Logging.ClearProviders();
// Add Serilog Library
builder.Logging.AddSerilog(logger);
    
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(MySQLConnection, 
        new MySqlServerVersion(new Version(8, 0, 28)))); // MySQL sürümünüzü belirtin

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders()
.AddTokenProvider<DataProtectorTokenProvider<AppUser>>("DataProtectorTokenProvider<AppUser>");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Demo API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddSingleton<IConnectionMultiplexer>(redisConnection);
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

builder.Services.AddTransient<ICurrencyService, CurrencyService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICalisanService, CalisanService>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICalisanRepository, CalisanRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["AppSettings:ValidIssuer"],
        ValidAudience = builder.Configuration["AppSettings:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<TokenAuthenticationMiddleware>();

app.MapControllers();

app.Run();