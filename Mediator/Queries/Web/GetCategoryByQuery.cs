using DotnetApiPostgres.Api.Mediator.Common;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Repository;
using MediatR;

namespace DotnetApiPostgres.Api.Mediator.Queries.Web
{
    public sealed class GetCategoryByQuery : ICommand<PostgresDataSource<Category>>
    {
        public PostgresQuery Query { get; set; } = new PostgresQuery();
    }

    public sealed class GetCategoryByQueryHandler : ICommandHandler<GetCategoryByQuery, PostgresDataSource<Category>>
    {
        private readonly IPostgresRepository<Category, string> _repository;
        private readonly IMediator _mediator;
        public GetCategoryByQueryHandler(
            IPostgresRepository<Category, string> repository,
            IMediator mediator
        )
        {
            _repository = repository;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PostgresDataSource<Category>> Handle(GetCategoryByQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByQueryAsync(request.Query);
        }
    }
}

