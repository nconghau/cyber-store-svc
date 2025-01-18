using System.Text.Json;
using Bogus;
using DotnetApiPostgres.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiPostgres.Api.Models;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly KafkaProducerService _kafkaProducerService;

    public OrderController(KafkaProducerService kafkaProducerService, ILogger<OrderController> logger)
    {
        _logger = logger;
        _kafkaProducerService = kafkaProducerService;
    }

    [HttpPost]
    public async Task<bool> TryKafkaCreateOrder()
    {
        // Faker instance for generating fake order data
        var faker = new Faker();

        for (var i = 0; i < 100; i++)
        {
            // Create a fake order object
            var order = new
            {
                OrderId = Guid.NewGuid().ToString(),
                CustomerName = faker.Name.FullName(),
                Email = faker.Internet.Email(),
                TotalAmount = faker.Random.Decimal(10, 1000),
                OrderDate = faker.Date.Recent(),
                Items = new[]
                {
                new
                {
                    ProductId = faker.Commerce.Ean13(),
                    ProductName = faker.Commerce.ProductName(),
                    Quantity = faker.Random.Int(1, 5),
                    Price = faker.Random.Decimal(5, 500)
                },
                new
                {
                    ProductId = faker.Commerce.Ean13(),
                    ProductName = faker.Commerce.ProductName(),
                    Quantity = faker.Random.Int(1, 5),
                    Price = faker.Random.Decimal(5, 500)
                }
            }
            };

            // Serialize the order to JSON
            var jsonOrder = JsonSerializer.Serialize(order);

            // Send the JSON order to Kafka
            _ = _kafkaProducerService.ProduceAsync("order-topic", jsonOrder);
            // await Task.Delay(100);
        }

        return true;
    }

}