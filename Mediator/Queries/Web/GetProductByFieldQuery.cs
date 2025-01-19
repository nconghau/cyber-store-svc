using DotnetApiPostgres.Api.Mappings;
using DotnetApiPostgres.Api.Mediator.Common;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Repository;
using FluentValidation;
using MediatR;

namespace DotnetApiPostgres.Api.Mediator.Queries.Web
{
    public sealed class GetProductByFieldQuery : IQuery<JsonResponse<ProductDto>>
    {
        public string Field { get; set; }
        public string Value { get; set; }
    }

    public sealed class GetProductByFieldQueryValidator : AbstractValidator<GetProductByFieldQuery>
    {
        public GetProductByFieldQueryValidator()
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

    public sealed class GetProductByFieldQueryHandler : IQueryHandler<GetProductByFieldQuery, JsonResponse<ProductDto>>
    {
        private readonly IPostgresRepository<Product, string> _repository;
        private readonly IMediator _mediator;
        public GetProductByFieldQueryHandler(
            IPostgresRepository<Product, string> repository,
            IMediator mediator
        )
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<JsonResponse<ProductDto>> Handle(GetProductByFieldQuery request, CancellationToken cancellationToken)
        {
            var response = new JsonResponse<ProductDto>();
            var data = await _repository.GetByFieldQueryAsync(request.Field, request.Value);

            if(data == null)
            {
                return response;
            }

            response.Success = true;
            response.Data = data.ToProductDto();
            return response;
        }
    }
}

