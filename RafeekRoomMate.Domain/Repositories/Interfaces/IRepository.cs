
using RafeekRoomMate.Domain.Specifications;

namespace RafeekRoomMate.Domain.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
