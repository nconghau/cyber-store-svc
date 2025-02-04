using System.Reflection;
using DotnetApiPostgres.Api;
using DotnetApiPostgres.Api.Repository;
using DotnetApiPostgres.Api.Services.Kafka;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// init APIs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

var enabledKafka = true;
var scriptMigration = false; // script: dotnet ef migrations add _ / dotnet ef database update

// env
string postgresConnection = builder.Configuration["PostgresConnection"];
string kafkaBroker = builder.Configuration["KafkaBroker"];
string kafkaTopic = builder.Configuration["KafkaTopic"];
var kafkaNumPartitions = int.Parse(builder.Configuration["KafkaNumPartitions"] ?? "1");
var kafkaNumConsumers = int.Parse(builder.Configuration["KafkaNumConsumers"] ?? "1");
string kafkaGroupId = builder.Configuration["KafkaGroupId"];

if (scriptMigration)
{
    postgresConnection = "Host=14.225.204.163;Port=5332;Database=cyber_store;Username=cyber_store;Password=cyber_store";
}

// postgres
using var connection = new NpgsqlConnection(postgresConnection);
connection.Open();
Console.WriteLine("PostgreSQL Connection successful!");
Console.WriteLine($"PostgreSQL version: {connection.PostgreSqlVersion}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(postgresConnection));
           //.EnableSensitiveDataLogging()
           //.LogTo(Console.WriteLine));

builder.Services.AddTransient(typeof(IPostgresRepository<,>), typeof(PostgresRepository<,>));


if (enabledKafka)
{
    // kafka
    builder.Services.AddSingleton(new KafkaAdminService(kafkaBroker));
    builder.Services.AddSingleton(new KafkaProducerService(kafkaBroker));
    builder.Services.AddSingleton(sp => new KafkaConsumerService(
        sp.GetRequiredService<IHttpClientFactory>(),
        kafkaBroker,
        kafkaTopic,
        kafkaNumConsumers,
        kafkaGroupId));
    builder.Services.AddHostedService<KafkaBackgroundService>();
}

// mediator
builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});


// init app 
var app = builder.Build();

// init kafka
if (enabledKafka)
{
    using (var scope = app.Services.CreateScope())
    {
        var adminService = scope.ServiceProvider.GetRequiredService<KafkaAdminService>();
        await adminService.CreateTopicAsync(kafkaTopic, kafkaNumPartitions, 1);
    }
}

//init postgres
DatabaseMigrationService.ApplyMigrations(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

