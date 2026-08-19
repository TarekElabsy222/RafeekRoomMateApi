using Microsoft.EntityFrameworkCore;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications;
using RafeekRoomMate.Infrastructure.Persistence.Context;
using RafeekRoomMate.Infrastructure.Specifications;

namespace RafeekRoomMate.Infrastructure.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region fields        
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        #endregion

        #region constructor        
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        #endregion

        #region handle functions
        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _dbSet.FindAsync(id, cancellationToken);


        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
           => await _dbSet.ToListAsync(cancellationToken);


        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id, cancellationToken) is not null;
        }

        #endregion

        #region Specification

        public async Task<T?> GetBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
            => await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);


        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
            => await ApplySpecification(specification).ToListAsync(cancellationToken);


        public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
            => await ApplySpecification(specification).CountAsync(cancellationToken);


        #endregion

        #region Private Methods

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), specification);
        }

        #endregion
    }
}
