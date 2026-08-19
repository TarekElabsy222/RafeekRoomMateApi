using Microsoft.EntityFrameworkCore.Storage;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Infrastructure.Persistence.Context;

namespace RafeekRoomMate.Infrastructure.Repositories.Implementations
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task BeginTransactionAsync( CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }
        public async Task CommitTransactionAsync(  CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                return;

            await _context.SaveChangesAsync(cancellationToken);

            await _transaction.CommitAsync(cancellationToken);

            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                return;

            await _transaction.RollbackAsync(cancellationToken);

            await _transaction.DisposeAsync();

            _transaction = null;
        }
    }
}
