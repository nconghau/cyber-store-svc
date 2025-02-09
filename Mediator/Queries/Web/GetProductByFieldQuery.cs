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
    public sealed class GetProductByFieldQuery : IQuery<JsonResponse<ProductDTO>>
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

    public sealed class GetProductByFieldQueryHandler : IQueryHandler<GetProductByFieldQuery, JsonResponse<ProductDTO>>
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

        public async Task<JsonResponse<ProductDTO>> Handle(GetProductByFieldQuery request, CancellationToken cancellationToken)
        {
            var response = new JsonResponse<ProductDTO>();
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

