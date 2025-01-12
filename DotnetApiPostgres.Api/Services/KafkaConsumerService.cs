using Confluent.Kafka;

namespace DotnetApiPostgres.Api.Services
{
    public class KafkaConsumerService : IHostedService
    {
        private readonly List<IConsumer<Ignore, string>> _consumers = new();
        private readonly string _topic;
        private readonly int _consumerCount;
        private readonly CancellationTokenSource _cts = new();

        public KafkaConsumerService(string bootstrapServers, string topic, int consumerCount)
        {
            _topic = topic;
            _consumerCount = consumerCount;

            // Initialize consumers based on the consumerCount
            for (int i = 0; i < _consumerCount; i++)
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = bootstrapServers,
                    GroupId = "order-topic-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
                _consumers.Add(consumer);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start consuming in parallel with passing the index to the consumer
            var consumeTasks = _consumers.Select((consumer, index) => Task.Run(() => ConsumeMessages(consumer, index + 1, cancellationToken))).ToList();

            // Return the task that tracks all consumers
            return Task.WhenAny(consumeTasks); // Allow any consumer to finish first, but they will keep running in the background.
        }

        private void ConsumeMessages(IConsumer<Ignore, string> consumer, int consumerIndex, CancellationToken cancellationToken)
        {
            consumer.Subscribe(_topic);
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    // Log the message with the consumer index (index starts from 1)
                    Console.WriteLine($"----\nMessage consumed (consumer {consumerIndex})::{consumeResult.Message.Value}");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error consuming message: {e.Error.Reason}");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Gracefully stop all consumers
            _cts.Cancel();
            foreach (var consumer in _consumers)
            {
                consumer.Close();
            }

            return Task.CompletedTask;
        }
    }
}