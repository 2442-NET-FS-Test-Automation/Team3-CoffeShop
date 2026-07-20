using CoffeShop.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conn_string = "Server=localhost,1433;Database=CoffeshopDB;User Id=sa;Password=LibPass123;TrustServerCertificate=true"; //Change it in each laptop

builder.Services.AddDbContextFactory<CoffeShopDbContext>(o => o.UseSqlServer(conn_string));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
