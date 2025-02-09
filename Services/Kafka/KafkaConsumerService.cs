using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using CyberStoreSVC.Mediator.Commands.Notify;
using CyberStoreSVC.Mediator.Commands.Web;
using CyberStoreSVC.Models.DTOs;
using MediatR;

namespace CyberStoreSVC.Services.Kafka
{
    public class KafkaConsumerService : IHostedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMediator _mediator;
        private readonly List<IConsumer<string, string>> _consumers = new();
        private readonly List<string> _topics = new List<string>();
        private readonly int _numConsumers;
        private readonly CancellationTokenSource _cts = new();
        private readonly string ApiUrlOrderSVC = "http://14.225.204.163:7295/api/Order/CreateOrder";

        public KafkaConsumerService(
            IHttpClientFactory httpClientFactory,
            IMediator mediator,
            string bootstrapServers,
            string topic,
            int numConsumers,
            string? groupId= "groupId_default"
        )
        {
            _httpClientFactory = httpClientFactory;
            _mediator = mediator;
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
                            var data = new CreateOrderCommand {
                                Data = JsonSerializer.Deserialize<OrderDTO>(consumeResult?.Message.Value)
                            };
                            if (data?.Data != null)
                            {
                                Task.Run(() => CallCreateOrder(data)); 
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

        private async Task CallCreateOrder(CreateOrderCommand data)
        {
            using var client = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(ApiUrlOrderSVC, jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    // Send message
                    var message = new StringBuilder();
                    var responseFail = await response.Content.ReadAsStringAsync();
                    message.AppendLine("❌ <b>Failed to create order!</b>");
                    message.AppendLine("=========================");
                    message.AppendLine($"Action: callCreateOrder");
                    message.AppendLine($"Status Code: {response.StatusCode}");
                    message.AppendLine($"Response: {responseFail}");
                    message.AppendLine("=========================");

                    _ = _mediator.Send(new SendTextMessageTelegramBotCommand()
                    {
                        Message = message.ToString()
                    });
                }
            }
            catch(Exception ex)
            {
                // Send message
                var message = new StringBuilder();
                message.AppendLine("❌ <b>Failed to create order!</b>");
                message.AppendLine("=========================");
                message.AppendLine($"Action: callCreateOrder");
                message.AppendLine($"Exception: {ex.Message}");
                message.AppendLine("=========================");

                _ = _mediator.Send(new SendTextMessageTelegramBotCommand()
                {
                    Message = message.ToString()
                });
            }
        }
    }
}