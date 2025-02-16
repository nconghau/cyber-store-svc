using CyberStoreSVC.Mediator.Common;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.Entities;
using CyberStoreSVC.Repository;
using CyberStoreSVC.Services.Cache;
using CyberStoreSVC.Utils;
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
        private readonly ICacheService _cacheService;
        public GetOrderByQueryHandler(
            IPostgresRepository<Order, string> repository,
            IMediator mediator,
            ICacheService cacheService
        )
        {
            _repository = repository;
            _mediator = mediator;
            _cacheService = cacheService;
        }

        public async Task<PostgresDataSource<Order>> Handle(GetOrderByQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = RedisKeyBuilder.GeneratePostgresQueryRedisKey("GetOrderByQuery", request.Query);
            var cacheValues = await _cacheService.GetAsync<List<string>>(cacheKey);

            if (cacheValues?.Count() > 0)
            {
                var newRequest = request.CloneJson();
                newRequest.Query.Criteria = new List<PostgresCriteria>()
                {
                    new PostgresCriteria()
                    {
                        Field = "id",
                        Value = cacheValues,
                        Type = "ids",
                    }
                };
                var resultByIds = await _repository.GetByQueryAsync(request.Query,
                                                                    null,
                                                                    new List<string>() {
                                                                          "OrderItems"
                                                                    });
                if (resultByIds.Success == true)
                {
                    // remove cache when data change
                    if (resultByIds?.Data?.Count() != cacheValues?.Count() || resultByIds?.Total != cacheValues?.Count())
                    {
                        _ = _cacheService.RemoveAsync(cacheKey);
                    }

                    resultByIds.Message += " (Cached)";
                    return resultByIds;
                }
            }

            var result = await _repository.GetByQueryAsync(request.Query,
                                                           null,
                                                           new List<string>() {
                                                                "OrderItems"
                                                           });
            if (result?.Data?.Count() > 0)
            {
                // cache orderIds
                _ = _cacheService.SetAsync(cacheKey, result.Data.Select(s => s.Id).ToList(), TimeSpan.FromMinutes(60));
            }

            return result;
        }
    }
}

