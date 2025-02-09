using CyberStoreSVC.Mediator.Common;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.Entities;
using CyberStoreSVC.Repository;
using MediatR;

namespace CyberStoreSVC.Mediator.Queries.Web
{
    public sealed class GetProductByQuery : IQuery<PostgresDataSource<Product>>
    {
        public PostgresQuery Query { get; set; } = new PostgresQuery();
    }

    public sealed class GetProductByQueryHandler : IQueryHandler<GetProductByQuery, PostgresDataSource<Product>>
    {
        private readonly IPostgresRepository<Product, string> _repository;
        private readonly IMediator _mediator;
        public GetProductByQueryHandler(
            IPostgresRepository<Product, string> repository,
            IMediator mediator
        )
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<PostgresDataSource<Product>> Handle(GetProductByQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByQueryAsync(request.Query);
        }
    }
}

