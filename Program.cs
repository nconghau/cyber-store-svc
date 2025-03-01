using CyberStoreSVC.Behaviors;
using CyberStoreSVC.Repository;
using CyberStoreSVC.Services.Common;
using CyberStoreSVC.Services.GoogleServices;
using CyberStoreSVC.Services.Kafka;
using CyberStoreSVC.Services.Redis;
using CyberStoreSVC.Services.TelegramBot;

// add services
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommonServices(builder.Configuration, builder.Logging);

// Try Https: Determine the certificate path dynamically
// var certPath = File.Exists("Private/certificate.pfx") ? "Private/certificate.pfx" : "/app/Private/certificate.pfx";
// Configure Kestrel for HTTPS with PFX
// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.Listen(IPAddress.Any, 7295, listenOptions =>
//     {
//         listenOptions.UseHttps(certPath, "cyber_store");
//     });
// });

var scriptMigrationPostgres = false; // script: dotnet ef migrations add _ / dotnet ef database update
builder.Services.AddPostgresServices(builder.Configuration, scriptMigrationPostgres);
if (!scriptMigrationPostgres)
{
    builder.Services.AddKafkaServices(builder.Configuration);
}
builder.Services.AddRedisServices();
builder.Services.AddGoogleDiagnosticsServices();
builder.Services.AddTelegramBotServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("default",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:4000",
                "https://localhost:4000",
                "http://localhost:3000",
                "https://localhost:3000",
                "https://localhost:3000",
                "http://14.225.204.163:3000",
                "http://14.225.204.163:4000"
                ) 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// use app 
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseCors("default");

DatabaseMigrationService.ApplyMigrations(app.Services);
if (!scriptMigrationPostgres)
{
    _ = app.UseKafkaApplicationAsync();
}

// run
app.Run();

