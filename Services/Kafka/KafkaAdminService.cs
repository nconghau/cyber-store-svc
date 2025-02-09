using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace CyberStoreSVC.Services.Kafka
{
    public class KafkaAdminService
    {
        private readonly string _bootstrapServers;

        public KafkaAdminService(string bootstrapServers)
        {
            _bootstrapServers = bootstrapServers;
        }

        public async Task CreateTopicAsync(string topicName, int numPartitions, short replicationFactor)
        {
            using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _bootstrapServers }).Build();
            try
            {
                await adminClient.CreateTopicsAsync(new List<TopicSpecification>
                {
                    new TopicSpecification
                    {
                        Name = topicName,
                        NumPartitions = numPartitions,
                        ReplicationFactor = replicationFactor
                    }
                });
                Console.WriteLine($"CreateTopicsAsync={topicName} created successfully.");
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"Error CreateTopicsAsync={topicName}::{e.Results[0].Error.Reason}");
            }
        }
    }
}

