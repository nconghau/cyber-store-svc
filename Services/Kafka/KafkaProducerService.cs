using Confluent.Kafka;

namespace DotnetApiPostgres.Api.Services.Kafka
{
    public class KafkaProducerService
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducerService(string bootstrapServers)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
            };

            try
            {
                _producer = new ProducerBuilder<string, string>(config).Build();

                using (var adminClient = new AdminClientBuilder(config).Build())
                {
                    var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));

                    Console.WriteLine("Kafka Connection successful!");

                    // Log brokers
                    Console.WriteLine("Kafka Available brokers:");
                    foreach (var broker in metadata.Brokers)
                    {
                        Console.WriteLine($"- {broker.Host}:{broker.Port}");
                    }

                    // Log topics
                    Console.WriteLine("Kafka Available topics:");
                    foreach (var topic in metadata.Topics)
                    {
                        if (topic.Error.IsError)
                        {
                            Console.WriteLine($"- {topic.Topic}: Error {topic.Error.Reason}");
                            continue;
                        }

                        Console.WriteLine($"- {topic.Topic}: {topic.Partitions.Count} partitions");
                        foreach (var partition in topic.Partitions)
                        {
                            Console.WriteLine($" Partition {partition.PartitionId}: Leader {partition.Leader}, Replicas {string.Join(", ", partition.Replicas)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kafka failed to connect to: {ex.Message}");
                throw;
            }
        }

        public async Task ProduceAsync(string topic, string message, string? key="")
        {
            try
            {
                var _key = string.IsNullOrEmpty(key) ?
                 $"{topic}_{((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds()}" :
                 key;

                var result = await _producer.ProduceAsync(
                    topic,
                    new Message<string, string>() {
                        Key = _key,
                        Value = message
                    });

                Console.WriteLine($"----\nProduceAsync::{result.Key}|{result.Value}");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Error ProduceAsync::{e.Message}");
            }
        }

        public void Close()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
        }
    }
}

