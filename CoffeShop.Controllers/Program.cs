using CoffeShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Serilog;
using System.Text;
using CoffeShop.Controllers.Services;
using Microsoft.Extensions.DependencyModel;
using System.IO.Compression;
using CoffeShop.Data.Enums;
using Microsoft.AspNetCore.Http.Connections;
using CoffeShop.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

//Db connection
var conn_string = "Server=localhost,1433;Database=coffeshopv2;User Id=sa;Password=LibPass123;TrustServerCertificate=true"; //Change it in each laptop

//Logger config
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/coffeshop-log.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

//CORS
const string SpaCorsPolicy = "spa";

//Cors Policy
builder.Services.AddCors(o => o.AddPolicy(SpaCorsPolicy, p => p
    .WithOrigins("http://localhost:5500")
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
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddDbContext<CoffeShopDbContext>(o => o.UseSqlServer(conn_string));

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

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

    if(!db.Users.Any(u => u.Role == RoleUsers.Manager))
    {
        
        var hasher = new PasswordHasher<User>();
        var admin = new User {Name = "Diego", Username = "Admin", Role = RoleUsers.Manager, Email = "Example@gmail.com"};

        var adminPassword = builder.Configuration["AdminConfig:DefaultPassword"];

        admin.PasswordHash = hasher.HashPassword(admin, adminPassword);

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

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();