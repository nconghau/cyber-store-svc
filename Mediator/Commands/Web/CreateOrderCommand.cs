using DotnetApiPostgres.Api.Mappings;
using DotnetApiPostgres.Api.Mediator.Common;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Repository;
using FluentValidation;
using MediatR;

namespace DotnetApiPostgres.Api.Mediator.Commands.Web
{
    public sealed class CreateOrderCommand : ICommand<JsonResponse<OrderDTO>>
    {
        public OrderDTO data { get; set; } 
    }

    public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.data)
                .NotNull()
                .NotEmpty()
                .WithMessage("data must not be null or empty.");
        }
    }

    public sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, JsonResponse<OrderDTO>>
    {
        private readonly IPostgresRepository<Order, string> _orderRepository;
        private readonly IMediator _mediator;
        public CreateOrderCommandHandler(
            IPostgresRepository<Order, string> orderRepository,
            IMediator mediator
        )
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public async Task<JsonResponse<OrderDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var response = new JsonResponse<OrderDTO>();
            try
            {
                var orderData = request.data.ToOrder();

                var data = await _orderRepository.AddAsync(orderData);

                //var data2 = await _orderRepository.AddAsync(new Order()
                //{
                //    Id = IdGenerator.GenerateId(),
                //    Email = "1",
                //    CustomerName = "1",
                //    OrderDate = DateTime.UtcNow,
                //    TotalAmount = 1
                //});

                if (data == null)
                {
                    return response;
                }

                response.Success = true;
                response.Data = request.data;
                return response;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }
    }
}

