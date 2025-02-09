using CyberStoreSVC.Mappings;
using CyberStoreSVC.Mediator.Common;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.DTOs;
using CyberStoreSVC.Models.Entities;
using CyberStoreSVC.Repository;
using FluentValidation;
using MediatR;

namespace CyberStoreSVC.Mediator.Queries.Web
{
    public sealed class GetOrderByFieldQuery : IQuery<JsonResponse<OrderDTO>>
    {
        public string Field { get; set; }
        public string Value { get; set; }
    }

    public sealed class GetOrderByFieldQueryValidator : AbstractValidator<GetOrderByFieldQuery>
    {
        public GetOrderByFieldQueryValidator()
        {
            RuleFor(x => x.Field)
                .NotNull()
                .NotEmpty()
                .WithMessage("Field must not be null or empty.");
            RuleFor(x => x.Value)
                .NotNull()
                .NotEmpty()
                .WithMessage("Value must not be null or empty.");
        }
    }

    public sealed class GetOrderByFieldQueryHandler : IQueryHandler<GetOrderByFieldQuery, JsonResponse<OrderDTO>>
    {
        private readonly IPostgresRepository<Order, string> _repository;
        private readonly IMediator _mediator;
        public GetOrderByFieldQueryHandler(
            IPostgresRepository<Order, string> repository,
            IMediator mediator
        )
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<JsonResponse<OrderDTO>> Handle(GetOrderByFieldQuery request, CancellationToken cancellationToken)
        {
            var response = new JsonResponse<OrderDTO>();
            var data = await _repository.GetByFieldQueryAsync(
                request.Field,
                request.Value,
                new List<string>() {
                    "OrderItems"
                }
            );

            if(data == null)
            {
                return response;
            }

            response.Success = true;
            response.Data = data.ToOrderDto();
            return response;
        }
    }
}

