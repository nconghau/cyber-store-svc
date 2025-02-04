using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using DotnetApiPostgres.Api.Mediator.Commands.Web;
using DotnetApiPostgres.Api.Models.DTOs;

namespace DotnetApiPostgres.Api.Services.Kafka
{
    public class KafkaConsumerService : IHostedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly List<IConsumer<string, string>> _consumers = new();
        private readonly List<string> _topics = new List<string>();
        private readonly int _numConsumers;
        private readonly CancellationTokenSource _cts = new();

        public KafkaConsumerService(IHttpClientFactory httpClientFactory, string bootstrapServers, string topic, int numConsumers, string? groupId= "groupId_default")
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _topics.Add(topic);
            _topics = _topics.Distinct().ToList();
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
            try
            {
                var consumeTasks = _consumers
                    .Select((consumer, index) =>
                        Task.Run(() =>
                            ConsumeMessages(
                                consumer,
                                index + 1,
                                cancellationToken
                            ))).ToList();

                return Task.WhenAny(consumeTasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error StartAsync::{ex.Message}");
                return Task.FromException(ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _cts.Cancel();
                foreach (var consumer in _consumers)
                {
                    consumer.Close();
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error StopAsync::{ex.Message}");
                return Task.FromException(ex);
            }
        }


        private void ConsumeMessages(IConsumer<string, string> consumer, int consumerIndex, CancellationToken cancellationToken)
        {
            consumer.Subscribe(_topics);
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    Console.WriteLine($"----\nConsumeMessages[{consumerIndex}]::{consumeResult?.Message?.Key}|{consumeResult?.Message.Value}");
                    switch (consumeResult.Topic)
                    {
                        case "k_order_2":
                            var data = new CreateOrderCommand { data = JsonSerializer.Deserialize<OrderDTO>(consumeResult?.Message.Value) };
                            if (data.data != null)
                            {
                                Task.Run(() => CallCreateOrderApi(data)); 
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Error ConsumeMessages::{ex.Error.Reason}");
                }
            }
        }

        private async Task CallCreateOrderApi(CreateOrderCommand data)
        {
            using var client = _httpClientFactory.CreateClient();
            var apiUrl = "https://localhost:7294/api/Order/CreateOrder"; 
            var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, jsonContent);

            // handle notify tele bot
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ Order created successfully!");
            }
            else
            {
                Console.WriteLine($"❌ Failed to create order. Status Code: {response.StatusCode}");
            }
        }
    }
}