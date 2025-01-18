using System.Text.Json;
using Bogus;
using DotnetApiPostgres.Api.Services.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiPostgres.Api.Models;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly KafkaProducerService _kafkaProducerService;
    private readonly IConfiguration _configuration;
    public OrderController(IConfiguration configuration, KafkaProducerService kafkaProducerService)
    {
        _kafkaProducerService = kafkaProducerService;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<bool> TryKafkaCreateOrder()
    {
        var faker = new Faker();

        for (var i = 0; i < 10; i++)
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
            var kafkaTopic = _configuration["KafkaTopic"];
            _ = _kafkaProducerService.ProduceAsync(kafkaTopic, jsonOrder);
            // await Task.Delay(100);
        }

        return true;
    }

}