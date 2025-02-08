using MediatR;

namespace DotnetApiPostgres.Api.Services.Kafka
{
    public static class KafkaRegistration
    {
        public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
        {
            string kafkaBroker = configuration["KafkaBroker"] ?? "";
            string kafkaTopic = configuration["KafkaTopic"] ?? "";
            int kafkaNumPartitions = int.TryParse(configuration["KafkaNumPartitions"], out var partitions) ? partitions : 1;
            int kafkaNumConsumers = int.TryParse(configuration["KafkaNumConsumers"], out var consumers) ? consumers : 1;
            string kafkaGroupId = configuration["KafkaGroupId"] ?? "";

            services.AddSingleton(new KafkaAdminService(kafkaBroker));
            services.AddSingleton(new KafkaProducerService(kafkaBroker));
            services.AddSingleton(sp => new KafkaConsumerService(
                sp.GetRequiredService<IHttpClientFactory>(),
                sp.GetRequiredService<IMediator>(),
                kafkaBroker,
                kafkaTopic,
                kafkaNumConsumers,
                kafkaGroupId));
            services.AddHostedService<KafkaBackgroundService>();

            services.AddSingleton(new KafkaConfig
            {
                Topic = kafkaTopic,
                NumPartitions = kafkaNumPartitions
            });

            return services;
        }

        public static async Task<IApplicationBuilder> UseKafkaApplicationAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var adminService = scope.ServiceProvider.GetRequiredService<KafkaAdminService>();
                var kafkaConfig = scope.ServiceProvider.GetRequiredService<KafkaConfig>();

                await adminService.CreateTopicAsync(kafkaConfig.Topic, kafkaConfig.NumPartitions, 1);
            }
            return app;
        }
    }

    public class KafkaConfig
    {
        public string Topic { get; set; } = "";
        public int NumPartitions { get; set; } = 1;
    }
}