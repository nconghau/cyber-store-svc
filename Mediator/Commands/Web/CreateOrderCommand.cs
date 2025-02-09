using System.Text;
using CyberStoreSVC.Mappings;
using CyberStoreSVC.Mediator.Commands.Notify;
using CyberStoreSVC.Mediator.Common;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.DTOs;
using CyberStoreSVC.Models.Entities;
using CyberStoreSVC.Repository;
using CyberStoreSVC.Utils;
using FluentValidation;
using MediatR;

namespace CyberStoreSVC.Mediator.Commands.Web
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
        private readonly IPostgresRepository<Models.Entities.Order, string> _orderRepository;
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

                // Send message
                var message = new StringBuilder();
                message.AppendLine("✅ <b>Order created successfully!</b>");
                message.AppendLine("=========================");

                foreach (var property in typeof(Order).GetProperties())
                {
                    object rawValue = property.GetValue(orderData);
                    switch (property.Name)
                    {
                        case "OrderDate":
                            if (rawValue is long timestamp)
                            {
                                var dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).ToLocalTime().DateTime;
                                message.AppendLine($"- {property.Name}: <b>{dateTime.ToString("HH:mm dd/MM/yyyy")}</b>");
                            }
                            break;
                        case "OrderItems":
                            if (orderItems.Any())
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

                response.Success = true;
                response.Data = request.Data;
                return response;

            }
            catch (Exception ex)
            {
                var message = new StringBuilder();
                message.AppendLine("❌ <b>Failed to create order!</b>");
                message.AppendLine("=========================");
                message.AppendLine($"Action: CreateOrderCommand");
                message.AppendLine($"Error: {ex.Message}");
                message.AppendLine("=========================");

                _ = _mediator.Send(new SendTextMessageTelegramBotCommand()
                {
                    Message = message.ToString()
                });

                response.Message = "Order creation failed due to an internal error.";

                await transaction.RollbackAsync(); // Rollback if any error occurs

                return response;
            }
        }
    }
}