using System.Text.Json;
using Bogus;
using CyberStoreSVC.Mediator.Commands.Web;
using CyberStoreSVC.Mediator.Queries.Web;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.DTOs;
using CyberStoreSVC.Models.Entities;
using CyberStoreSVC.Services.Kafka;
using CyberStoreSVC.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CyberStoreSVC.Models;

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

    [HttpPost]
    public async Task<PostgresDataSource<Order>> GetOrderByQuery([FromBody] PostgresQuery query)
    {
        var jsonResponse = await _mediator.Send(new GetOrderByQuery()
        {
            Query = query
        });
        return jsonResponse;
    }

    [HttpPost]
    public async Task<JsonResponse<OrderDTO>> GetOrderByField([FromBody] GetOrderByFieldQuery data)
    {
        var jsonResponse = await _mediator.Send(data);
        return jsonResponse;
    }
}