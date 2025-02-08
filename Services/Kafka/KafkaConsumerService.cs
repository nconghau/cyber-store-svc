using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using DotnetApiPostgres.Api.Mediator.Commands.Notify;
using DotnetApiPostgres.Api.Mediator.Commands.Web;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using MediatR;

namespace DotnetApiPostgres.Api.Services.Kafka
{
    public class KafkaConsumerService : IHostedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMediator _mediator;
        private readonly List<IConsumer<string, string>> _consumers = new();
        private readonly List<string> _topics = new List<string>();
        private readonly int _numConsumers;
        private readonly CancellationTokenSource _cts = new();

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
                            var data = new CreateOrderCommand { Data = JsonSerializer.Deserialize<OrderDTO>(consumeResult?.Message.Value) };
                            if (data?.Data != null)
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
            var apiUrl = "https://localhost:7294/api/Order/CreateOrder"; // change to Domain
            var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = JsonSerializer.Deserialize<JsonResponse<OrderDTO>>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (jsonResponse?.Data != null)
                    {
                        var order = jsonResponse.Data;

                        var message = new StringBuilder();
                        message.AppendLine("✅ <b>Order created successfully!</b>");
                        message.AppendLine("=========================");

                        foreach (var property in typeof(OrderDTO).GetProperties())
                        {
                            object rawValue = property.GetValue(order);
                            switch (property.Name)
                            {
                                case "OrderDate":
                                    if(rawValue is long timestamp)
                                    {
                                        var dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).ToLocalTime().DateTime;
                                        message.AppendLine($"- {property.Name}: <b>{dateTime.ToString("HH:mm dd/MM/yyyy")}</b>");
                                    }
                                    break;
                                case "OrderItems":
                                    if (rawValue is List<OrderItemDTO> orderItems && orderItems.Any())
                                    {
                                        var orderItemsString = string.Join(Environment.NewLine,
                                            orderItems.Select(item =>
                                                $"\n\t+ ProductName: <b>{item.ProductName}</b>" +
                                                $"\n\t+ ProductId: <b>{item.ProductId}</b>" +
                                                $"\n\t+ Price: <b>{item.Price}</b>" +
                                                $"\n\t+ Qty: <b>{item.Qty}</b>"
                                            )
                                        );
                                        message.AppendLine($"- {property.Name}: {orderItemsString}");
                                    }
                                    else
                                    {
                                        message.AppendLine($"- {property.Name}: <b>No items</b>");
                                    }
                                    break;
                                default:
                                    var value = rawValue?.ToString() ?? "N/A";
                                    message.AppendLine($"- {property.Name}: <b>{value}</b>");
                                    break;
                            }
                        }

                        message.AppendLine("=========================");

                        // Send message
                        _ = _mediator.Send(new SendTextMessageTelegramBotCommand()
                        {
                            Message = message.ToString()
                        });
                    }
                }
                else
                {
                    // Send message
                    var message = new StringBuilder();
                    var responseFail = await response.Content.ReadAsStringAsync();
                    message.AppendLine("❌ <b>Failed to create order!</b>");
                    message.AppendLine("=========================");
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