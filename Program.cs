using DotnetApiPostgres.Api.Repository;
using DotnetApiPostgres.Api.Services.Common;
using DotnetApiPostgres.Api.Services.GoogleServices;
using DotnetApiPostgres.Api.Services.Kafka;
using DotnetApiPostgres.Api.Services.Redis;
using DotnetApiPostgres.Api.Services.TelegramBot;

// add services
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCommonServices(builder.Configuration);

var scriptMigrationPostgres = false; // script: dotnet ef migrations add _ / dotnet ef database update
builder.Services.AddPostgresServices(builder.Configuration, scriptMigrationPostgres);
if (!scriptMigrationPostgres)
{
    builder.Services.AddKafkaServices(builder.Configuration);
}
builder.Services.AddRedisServices(builder.Configuration);
builder.Services.AddGoogleDiagnosticsServices();
builder.Services.AddTelegramBotServices();

// use app 
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

DatabaseMigrationService.ApplyMigrations(app.Services);
if (!scriptMigrationPostgres)
{
    _ = app.UseKafkaApplicationAsync();
}

// run
app.Run();

