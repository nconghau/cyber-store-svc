using System.Reflection;
using DotnetApiPostgres.Api;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Repository;
using DotnetApiPostgres.Api.Services.Kafka;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static DotnetApiPostgres.Api.ApplicationDbContext;

var builder = WebApplication.CreateBuilder(args);

// init APIs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// env
string connectionString = builder.Configuration["PostgresConnection"] ?? throw new ArgumentNullException("PostgresConnection is not configured.");
string kafkaBroker = builder.Configuration["KafkaBroker"] ?? throw new ArgumentNullException("KafkaBroker is not configured.");
string kafkaTopic = builder.Configuration["KafkaTopic"] ?? throw new ArgumentNullException("KafkaTopic is not configured.");
var kafkaNumPartitions = int.Parse(builder.Configuration["KafkaNumPartitions"] ?? "1");
var kafkaNumConsumers = int.Parse(builder.Configuration["KafkaNumConsumers"] ?? "1");
string kafkaGroupId = builder.Configuration["KafkaGroupId"] ?? throw new ArgumentNullException("KafkaGroupId is not configured.");

// postgres
using var connection = new NpgsqlConnection(connectionString);
connection.Open();
Console.WriteLine("PostgreSQL Connection successful!");
Console.WriteLine($"PostgreSQL version: {connection.PostgreSqlVersion}");

builder.Services.AddDbContext<ApplicationDbContext>(op => op.UseNpgsql(connectionString));
builder.Services.AddScoped(typeof(IPostgresRepository<,>), typeof(PostgresRepository<,>));
DynamicEntityRegistry.AddEntity<Category>();

// kafka
builder.Services.AddSingleton(new KafkaAdminService(kafkaBroker));
builder.Services.AddSingleton(new KafkaProducerService(kafkaBroker));  
builder.Services.AddSingleton(new KafkaConsumerService(kafkaBroker, kafkaTopic, kafkaNumConsumers, kafkaGroupId)); 
builder.Services.AddHostedService<KafkaBackgroundService>();

// mediator
builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});

// init app 
var app = builder.Build();

// init kafka
using (var scope = app.Services.CreateScope())
{
    var adminService = scope.ServiceProvider.GetRequiredService<KafkaAdminService>();
    await adminService.CreateTopicAsync(kafkaTopic, kafkaNumPartitions, 1); 
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

