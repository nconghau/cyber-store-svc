using System.Text;
using DotnetApiPostgres.Api.Mappings;
using DotnetApiPostgres.Api.Mediator.Commands.Notify;
using DotnetApiPostgres.Api.Mediator.Common;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Repository;
using DotnetApiPostgres.Api.Utils;
using FluentValidation;
using MediatR;

namespace DotnetApiPostgres.Api.Mediator.Commands.Web
{
    public sealed class CreateOrderCommand : ICommand<JsonResponse<OrderDTO>>
    {
        public OrderDTO Data { get; set; }
    }

    public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Data)
                .NotNull()
                .WithMessage("Order data must not be null.");

            RuleFor(x => x.Data.Phone)
                .NotEmpty()
                .WithMessage("Phone name is required.");

            RuleFor(x => x.Data.OrderAddress)
               .NotEmpty()
               .WithMessage("OrderAddress name is required.");

            RuleFor(x => x.Data.CustomerName)
                .NotEmpty()
                .WithMessage("Customer name is required.");

            //RuleFor(x => x.Data.OrderItems)
            //    .NotNull()
            //    .NotEmpty()
            //    .WithMessage("Order must contain at least one order item.");
        }
    }

    public sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, JsonResponse<OrderDTO>>
    {
        private readonly IPostgresRepository<Order, string> _orderRepository;
        private readonly IPostgresRepository<OrderItem, string> _orderItemRepository;
        private readonly IMediator _mediator;
        public CreateOrderCommandHandler(
            IPostgresRepository<Order, string> orderRepository,
            IPostgresRepository<OrderItem, string> orderItemRepository,
            IMediator mediator
        )
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _mediator = mediator;
        }

        public async Task<JsonResponse<OrderDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var response = new JsonResponse<OrderDTO>();

            using var transaction = await _orderRepository.BeginTransactionAsync();
            try
            {
                var orderData = request.Data.ToOrder();

                await _orderRepository.AddAsync(orderData);

                var orderItems = request.Data.OrderItems
                    .Select(item => new OrderItem
                    {
                        Id = IdGenerator.GenerateId(),
                        OrderId = orderData.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Qty = item.Qty,
                    }).ToList();

                await _orderItemRepository.AddManyAsync(orderItems);

                // Save all changes in a single transaction
                await _orderRepository.SaveChangesAsync();
                await transaction.CommitAsync(); 

                response.Success = true;
                response.Data = request.Data;
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback if any error occurs

                var message = new StringBuilder();
                message.AppendLine("❌ <b>Failed to create order!</b>");
                message.AppendLine("=========================");
                message.AppendLine($"Error: {ex.Message}");
                message.AppendLine("=========================");

                _ = _mediator.Send(new SendTextMessageTelegramBotCommand()
                {
                    Message = message.ToString()
                });

                response.Message = "Order creation failed due to an internal error.";
                return response;
            }
        }
    }
}