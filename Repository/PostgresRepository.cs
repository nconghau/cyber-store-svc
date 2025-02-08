using System.Linq.Expressions;
using DotnetApiPostgres.Api.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DotnetApiPostgres.Api.Repository
{
    public interface IPostgresRepository<TEntity, TKey> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(Func<TEntity, bool> filter);
        Task<TEntity?> FindByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddManyAsync(IEnumerable<TEntity> entities);
        Task UpdateManyAsync(IEnumerable<TEntity> entities);
        Task<PostgresDataSource<TEntity>> GetByQueryAsync(PostgresQuery query, Func<TEntity, bool>? filter = null);
        Task<TEntity?> GetByFieldQueryAsync(string field, object value);
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }

    public class PostgresRepository<TEntity, TKey> : IPostgresRepository<TEntity, TKey> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public PostgresRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Func<TEntity, bool> filter)
        {
            var entitiesToDelete = _dbSet.Where(filter).ToList();
            if (entitiesToDelete.Any())
            {
                _dbSet.RemoveRange(entitiesToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TEntity?> FindByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddManyAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateManyAsync(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<PostgresDataSource<TEntity>> GetByQueryAsync(PostgresQuery query, Func<TEntity, bool> filter = null)
        {
            try
            {
                // Start with the base query (DbSet)
                IQueryable<TEntity> queryable = _dbSet;

                // Apply the criteria dynamically
                if (query.Criteria.Any())
                {
                    var parameter = Expression.Parameter(typeof(TEntity), "e");
                    Expression combinedExpression = null;

                    foreach (var criteria in query.Criteria)
                    {
                        var property = Expression.Property(parameter, criteria.Field);
                        Expression filterExpression = criteria.Type switch
                        {
                            "equal" => Expression.Equal(property, Expression.Constant(criteria.Value.ToString())),
                            //"list" when criteria.Value is IEnumerable<string> list =>
                            //    Expression.Call(
                            //        typeof(Enumerable),
                            //        "Contains",
                            //        new[] { property.Type },
                            //        Expression.Constant(list),
                            //        property),
                            "range" when criteria.Value is List<int> range && range.Count == 2 =>
                                Expression.AndAlso(
                                    Expression.GreaterThanOrEqual(property, Expression.Constant(range[0])),
                                    Expression.LessThanOrEqual(property, Expression.Constant(range[1]))),
                            _ => throw new InvalidOperationException($"Unsupported criteria type: {criteria.Type}")
                        };

                        if (combinedExpression == null)
                        {
                            combinedExpression = filterExpression;
                        }
                        else
                        {
                            combinedExpression = query.Operator.ToLower() == "and"
                                ? Expression.AndAlso(combinedExpression, filterExpression)
                                : Expression.OrElse(combinedExpression, filterExpression);
                        }
                    }

                    var lambda = Expression.Lambda<Func<TEntity, bool>>(combinedExpression, parameter);
                    queryable = queryable.Where(lambda);
                }

                // Apply the custom filter if provided
                if (filter != null)
                {
                    queryable = queryable.Where(filter).AsQueryable();
                }

                // Apply sorting dynamically using reflection
                if (!string.IsNullOrEmpty(query?.Sort?.Field))
                {
                    var sortField = query.Sort.Field;
                    var sortOrder = query.Sort.Order.ToLower();

                    var parameter = Expression.Parameter(typeof(TEntity), "e");
                    var property = Expression.Property(parameter, sortField);
                    var lambda = Expression.Lambda(property, parameter);

                    var methodName = sortOrder == "desc" ? "OrderByDescending" : "OrderBy";
                    var method = typeof(Queryable).GetMethods()
                        .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                        .MakeGenericMethod(typeof(TEntity), property.Type);

                    queryable = (IQueryable<TEntity>)method.Invoke(null, new object[] { queryable, lambda });
                }

                // Apply pagination (skip and take)
                var totalRecords = await queryable.CountAsync();
                var pagedData = await queryable.Skip((query.PageNumber - 1) * query.PageSize)
                                               .Take(query.PageSize)
                                               .ToListAsync();

                // Create the result object
                var dataSource = new PostgresDataSource<TEntity>
                {
                    Total = totalRecords,
                    PageNumber = query.PageNumber,
                    PageSize = query.PageSize,
                    Data = pagedData,
                    Message = "Query executed successfully"
                };

                return dataSource;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return new PostgresDataSource<TEntity>
                {
                    Success = false,
                    Message = "An error occurred while processing the query. " + ex.Message
                };
            }
        }


        public async Task<TEntity?> GetByFieldQueryAsync(string field, object value)
        {
            try
            {
                var parameter = Expression.Parameter(typeof(TEntity), "e");
                var property = Expression.Property(parameter, field);
                var constant = Expression.Constant(value);

                var equalExpression = Expression.Equal(property, constant);

                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);

                var entity = await _dbSet.FirstOrDefaultAsync(lambda);

                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
