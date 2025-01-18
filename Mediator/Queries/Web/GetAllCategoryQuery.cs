using DotnetApiPostgres.Api.Mappings;
using DotnetApiPostgres.Api.Mediator.Common;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Repository;
using MediatR;

namespace DotnetApiPostgres.Api.Mediator.Queries.Web
{
    public sealed class GetAllCategoryQuery : ICommand<JsonResponse<List<CategoryDto>>>
    {

    }

    public sealed class GetAllCategoryQueryHandler : ICommandHandler<GetAllCategoryQuery, JsonResponse<List<CategoryDto>>>
    {
        private readonly IPostgresRepository<Category, string> _repository;
        private readonly IMediator _mediator;
        public GetAllCategoryQueryHandler(
            IPostgresRepository<Category, string> repository,
            IMediator mediator
        )
        {
            _repository = repository;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<JsonResponse<List<CategoryDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var response = new JsonResponse<List<CategoryDto>>();
            var categories = await _repository.GetAllAsync();

            response.Success = true;
            response.Data = categories.Select(p => p.ToCategoryDto()).ToList();
            return response;
        }
    }
}

