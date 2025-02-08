using System.Text.Json;
using Bogus;
using DotnetApiPostgres.Api.Mediator.Commands.Web;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Services.Kafka;
using DotnetApiPostgres.Api.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiPostgres.Api.Models;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly KafkaProducerService _kafkaProducerService;
    private readonly IConfiguration _configuration;
    public OrderController(IMediator mediator, IConfiguration configuration, KafkaProducerService kafkaProducerService)
    {
        _mediator = mediator;
        _kafkaProducerService = kafkaProducerService;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<bool> TryKafkaCreateOrder()
    {
        var faker = new Faker("vi");

        for (var i = 0; i < 1; i++)
        {
            // Create a fake order object
            var data = new OrderDTO
            {
                Id = IdGenerator.GenerateId(),
                CustomerName = faker.Name.FullName(), // Generates a Vietnamese name
                Email = faker.Internet.Email(),
                Phone = DataGenerator.GenerateVietnamesePhoneNumber(faker),
                OrderAddress = faker.Address.FullAddress(),
                TotalAmount = (int) faker.Random.Decimal(100000, 5000000), // Vietnamese currency range (VND)
                OrderDate = new DateTimeOffset(faker.Date.Recent()).ToUnixTimeSeconds(),
                OrderItems = DataGenerator.GenerateFakeOrderItems(1, 3) // Generate 1-3 order items
            };

            var kafkaTopic = _configuration["KafkaTopic"];
            var jsonOrder = JsonSerializer.Serialize(data);
            _ = _kafkaProducerService.ProduceAsync(kafkaTopic, jsonOrder);
        }

        return true;
    }


    [HttpPost]
    public async Task<JsonResponse<OrderDTO>> CreateOrder([FromBody] CreateOrderCommand data)
    {
        var jsonResponse = await _mediator.Send(data);
        return jsonResponse;
    }

}