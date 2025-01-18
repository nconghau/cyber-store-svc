using Confluent.Kafka;

namespace DotnetApiPostgres.Api.Services.Kafka
{
    public class KafkaConsumerService : IHostedService
    {
        private readonly List<IConsumer<string, string>> _consumers = new();
        private readonly string _topic;
        private readonly int _numConsumers;
        private readonly CancellationTokenSource _cts = new();

        public KafkaConsumerService(string bootstrapServers, string topic, int numConsumers, string? groupId= "groupId_default")
        {
            _topic = topic;
            _numConsumers = numConsumers;
            for (int i = 0; i < _numConsumers; i++)
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = bootstrapServers,
                    GroupId = groupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = true,
                };

                var consumer = new ConsumerBuilder<string, string>(config).Build();
                _consumers.Add(consumer);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumeTasks = _consumers.Select((consumer, index) =>
                Task.Run(() => ConsumeMessages(consumer, index + 1, cancellationToken))).
                ToList();

            return Task.WhenAny(consumeTasks);
        }

        private void ConsumeMessages(IConsumer<string, string> consumer, int consumerIndex, CancellationToken cancellationToken)
        {
            consumer.Subscribe(_topic);
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    Console.WriteLine($"----\nConsumeMessages[{consumerIndex}]::{consumeResult?.Message?.Key}|{consumeResult?.Message.Value}");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error ConsumeMessages::{e.Error.Reason}");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            foreach (var consumer in _consumers)
            {
                consumer.Close();
            }

            return Task.CompletedTask;
        }
    }
}