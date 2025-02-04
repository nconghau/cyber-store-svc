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
        var faker = new Faker();

        for (var i = 0; i < 10; i++)
        {
            // Create a fake order object
            var data = new OrderDTO
            {
                Id = IdGenerator.GenerateId(),
                CustomerName = faker.Name.FullName(),
                Email = faker.Internet.Email(),
                TotalAmount = (int)faker.Random.Decimal(10, 1000),
                OrderDate = new DateTimeOffset(faker.Date.Recent()).ToUnixTimeSeconds()
                //ShipMethod = faker.PickRandom(new[] { "Standard", "Express", "Next-Day" }),
                //Items = Enumerable.Range(1, faker.Random.Int(1, 3))
                //        .Select(_ => new ItemDTO
                //                {
                //                    ProductId = faker.Commerce.Ean13(),
                //                    ProductName = faker.Commerce.ProductName(),
                //                    Quantity = faker.Random.Int(1, 5),
                //                    Price = faker.Random.Decimal(5, 500)
                //                })
                //        .ToList(),
                //Address = new AddressDTO
                //{
                //    Street = faker.Address.StreetAddress(),
                //    City = faker.Address.City(),
                //    State = faker.Address.State(),
                //    PostalCode = faker.Address.ZipCode(),
                //    Country = faker.Address.Country()
                //}
            };

            var kafkaTopic = _configuration["KafkaTopic"];
            var jsonOrder = JsonSerializer.Serialize(data);
            _ = _kafkaProducerService.ProduceAsync(kafkaTopic, jsonOrder);

            //await Task.Delay(100);
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