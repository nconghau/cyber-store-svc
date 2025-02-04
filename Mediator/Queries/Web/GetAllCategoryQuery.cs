using DotnetApiPostgres.Api.Mappings;
using DotnetApiPostgres.Api.Mediator.Common;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Repository;
using MediatR;

namespace DotnetApiPostgres.Api.Mediator.Queries.Web
{
    public sealed class GetAllCategoryQuery : IQuery<JsonResponse<List<CategoryDTO>>>
    {

    }

    public sealed class GetAllCategoryQueryHandler : IQueryHandler<GetAllCategoryQuery, JsonResponse<List<CategoryDTO>>>
    {
        private readonly IPostgresRepository<Category, string> _repository;
        private readonly IMediator _mediator;
        public GetAllCategoryQueryHandler(
            IPostgresRepository<Category, string> repository,
            IMediator mediator
        )
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<JsonResponse<List<CategoryDTO>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var response = new JsonResponse<List<CategoryDTO>>();
            var datas = await _repository.GetAllAsync();

            response.Success = true;
            response.Data = datas.Select(p => p.ToCategoryDto()).ToList();
            return response;
        }
    }
}

