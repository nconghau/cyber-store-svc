using DotnetApiPostgres.Api;
using DotnetApiPostgres.Api.Models;
using DotnetApiPostgres.Api.Repository;
using DotnetApiPostgres.Api.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static DotnetApiPostgres.Api.ApplicationDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

string connectionString = builder.Configuration.GetConnectionString("default");

using var connection = new NpgsqlConnection(connectionString);
connection.Open();
Console.WriteLine("PostgreSQL Connection successful!");
Console.WriteLine($"PostgreSQL version: {connection.PostgreSqlVersion}");

builder.Services.AddDbContext<ApplicationDbContext>(op => op.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(IPostgresRepository<,>), typeof(PostgresRepository<,>));

DynamicEntityRegistry.AddEntity<Category>();

// for testing
builder.Services.AddTransient<IPersonService, PersonService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

