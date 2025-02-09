using CyberStoreSVC.Mediator.Common;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.Entities;
using CyberStoreSVC.Repository;
using MediatR;

namespace CyberStoreSVC.Mediator.Queries.Web
{
    public sealed class GetOrderByQuery : IQuery<PostgresDataSource<Order>>
    {
        public PostgresQuery Query { get; set; } = new PostgresQuery();
    }

    public sealed class GetOrderByQueryHandler : IQueryHandler<GetOrderByQuery, PostgresDataSource<Order>>
    {
        private readonly IPostgresRepository<Order, string> _repository;
        private readonly IMediator _mediator;
        public GetOrderByQueryHandler(
            IPostgresRepository<Order, string> repository,
            IMediator mediator
        )
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<PostgresDataSource<Order>> Handle(GetOrderByQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByQueryAsync(
                request.Query,
                null,
                 new List<string>() {
                    "OrderItems"
                }
          );
        }
    }
}

