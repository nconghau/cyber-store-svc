using DotnetApiPostgres.Api.Mappings;
using DotnetApiPostgres.Api.Mediator.Common;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Repository;
using MediatR;

namespace DotnetApiPostgres.Api.Mediator.Queries.Web
{
    public sealed class GetAllProductQuery : IQuery<JsonResponse<List<ProductDto>>>
    {

    }

    public sealed class GetAllProductQueryHandler : IQueryHandler<GetAllProductQuery, JsonResponse<List<ProductDto>>>
    {
        private readonly IPostgresRepository<Product, string> _repository;
        private readonly IMediator _mediator;
        public GetAllProductQueryHandler(
            IPostgresRepository<Product, string> repository,
            IMediator mediator
        )
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<JsonResponse<List<ProductDto>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var response = new JsonResponse<List<ProductDto>>();
            var datas = await _repository.GetAllAsync();

            response.Success = true;
            response.Data = datas.Select(p => p.ToProductDto()).ToList();
            return response;
        }
    }
}

