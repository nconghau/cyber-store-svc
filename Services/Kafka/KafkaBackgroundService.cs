namespace CyberStoreSVC.Services.Kafka
{
    public class KafkaBackgroundService : BackgroundService
    {
        private readonly KafkaConsumerService _kafkaConsumerService;

        public KafkaBackgroundService(KafkaConsumerService kafkaConsumerService)
        {
            _kafkaConsumerService = kafkaConsumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _kafkaConsumerService.StartAsync(stoppingToken);
        }
    }
}

