using Microsoft.EntityFrameworkCore;

namespace DotnetApiPostgres.Api.Repository
{
    public interface IPostgresRepository<TEntity, TKey> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity?> FindByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
    }

    public class PostgresRepository<TEntity, TKey> : IPostgresRepository<TEntity, TKey> where TEntity : class
    {
        private readonly ApplicationDbContext _context;  // Use ApplicationDbContext here
        private readonly DbSet<TEntity> _dbSet;

        public PostgresRepository(ApplicationDbContext context)  // Constructor now uses ApplicationDbContext
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
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

        public async Task<TEntity?> FindByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
    }
}
