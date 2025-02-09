using CyberStoreSVC.Mediator.Common;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.Entities;
using CyberStoreSVC.Repository;
using MediatR;

namespace CyberStoreSVC.Mediator.Queries.Web
{
    public sealed class GetCategoryByQuery : IQuery<PostgresDataSource<Category>>
    {
        public PostgresQuery Query { get; set; } = new PostgresQuery();
    }

    public sealed class GetCategoryByQueryHandler : IQueryHandler<GetCategoryByQuery, PostgresDataSource<Category>>
    {
        private readonly IPostgresRepository<Category, string> _repository;
        private readonly IMediator _mediator;
        public GetCategoryByQueryHandler(
            IPostgresRepository<Category, string> repository,
            IMediator mediator
        )
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<PostgresDataSource<Category>> Handle(GetCategoryByQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByQueryAsync(request.Query);
        }
    }
}

