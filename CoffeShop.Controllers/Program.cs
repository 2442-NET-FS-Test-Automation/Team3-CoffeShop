using CoffeShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using CoffeShop.Controllers.Services;
using CoffeShop.Data.Enums;
using CoffeShop.Data.Entities;
using CoffeShop.Controllers.Mapping;

var builder = WebApplication.CreateBuilder(args);

//Db connection
//var conn_string = "Server=localhost,1435;Database=coffeshopv2;User Id=sa;Password=Deniro_007;TrustServerCertificate=true"; //Change it in each laptop
var conn_string = builder.Configuration.GetConnectionString("CoffeShop") ??
"Server=localhost,1435;Database=coffeshopv2;User Id=sa;Password=Deniro_007;TrustServerCertificate=false";
// var conn_string =
//     builder.Configuration.GetConnectionString("CoffeShop")
//     ?? throw new InvalidOperationException(
//         "Connection string 'CoffeShop' not found.");

//Logger config
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/coffeshop-log.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

//CORS
const string SpaCorsPolicy = "spa";
//Legacy
//Cors Policy
// builder.Services.AddCors(o => o.AddPolicy(SpaCorsPolicy, p => p
//     .WithOrigins("http://localhost:5173")
//     .AllowAnyHeader()
//     .AllowAnyMethod()
//     ));


// Lets change our CORS Block - we will still have our dev origins from the above code
// but when the app is DEPLOYED - we want cors come in from env/config

var extraOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? Array.Empty<string>();

var spaOrigins = new[] { "http://localhost:5173" }
    .Concat(extraOrigins)
    .ToArray();

    builder.Services.AddCors(o => o.AddPolicy(SpaCorsPolicy, p => p
    .WithOrigins(spaOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod()    
));


//Validation JWT 
var jwtKey = builder.Configuration["Jwt:key"]; //Key from appsettings

const string jwtIssuer = "coffe-user";
const string jwtAudience = "coffe-role";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
        ValidateLifetime = true
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IReportsService, ReportsService>();

builder.Services.AddDbContext<CoffeShopDbContext>(o => o.UseSqlServer(conn_string));

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

var app = builder.Build();

//Seeding Admins to test my endpoint!!
using (var scope = app.Services.CreateScope())
{

    var db = scope.ServiceProvider.GetRequiredService<CoffeShopDbContext>();

    if (!db.Users.Any(u => u.Role == RoleUsers.Manager))
    {

        var hasher = new PasswordHasher<User>();
        var admin = new User { Name = "Diego", Username = "Admin", Role = RoleUsers.Manager, Email = "Example@gmail.com" };

        var adminPassword = builder.Configuration["AdminConfig:DefaultPassword"];

        admin.PasswordHash = hasher.HashPassword(admin, adminPassword!);

        db.Users.Add(admin);
        db.SaveChanges();
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(SpaCorsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
