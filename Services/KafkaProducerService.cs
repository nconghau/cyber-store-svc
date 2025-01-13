using Confluent.Kafka;

namespace DotnetApiPostgres.Api.Services
{
    public class KafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaProducerService(string bootstrapServers)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            try
            {
                // Create the producer
                _producer = new ProducerBuilder<Null, string>(config).Build();

                // Fetch metadata to verify connection and log details
                using (var adminClient = new AdminClientBuilder(config).Build())
                {
                    var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));

                    Console.WriteLine("Kafka Connection successful!");

                    // Log brokers
                    Console.WriteLine("Available brokers:");
                    foreach (var broker in metadata.Brokers)
                    {
                        Console.WriteLine($"- {broker.Host}:{broker.Port}");
                    }

                    // Log topics
                    Console.WriteLine("Available topics:");
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
                            Console.WriteLine($"  Partition {partition.PartitionId}: Leader {partition.Leader}, Replicas {string.Join(", ", partition.Replicas)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to Kafka: {ex.Message}");
                throw;
            }
        }

        public async Task ProduceAsync(string topic, string message)
        {
            try
            {
                var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                Console.WriteLine($"----\nMessage produced::{result.Value}");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Error producing message: {e.Message}");
            }
        }

        public void Close()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
        }
    }
}

