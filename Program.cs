using DotnetApiPostgres.Api;
using DotnetApiPostgres.Api.Models.Entities;
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

// kafka
//builder.Services.AddSingleton(new KafkaProducerService("localhost:9092"));  // Update with your Kafka server address
//builder.Services.AddSingleton(new KafkaConsumerService("localhost:9092", "order-topic", 3));  // Update with your topic

builder.Services.AddSingleton(new KafkaProducerService("14.225.204.163:9092"));  // Update with your Kafka server address
builder.Services.AddSingleton(new KafkaConsumerService("14.225.204.163:9092", "order-topic", 3));  // Update with your topic

builder.Services.AddHostedService<KafkaBackgroundService>(); // For consuming messages in background


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

