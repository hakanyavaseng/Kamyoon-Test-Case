using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProductManagement.Core.Interfaces.UnitOfWork;
using ProductManagement.Persistence.Contexts;

namespace ProductManagement.Persistence.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction is active.");
        await _transaction.CommitAsync();
    }

    public void Commit()
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction is active.");
        _transaction.Commit();
    }

    public async Task RollbackAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction is active.");
        await _transaction.RollbackAsync();
    }

    public void Rollback()
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction is active.");
        _transaction.Rollback();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return _transaction;
    }

    public IDbContextTransaction BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
        return _transaction;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null) await _transaction.DisposeAsync();
        await _context.DisposeAsync();
    }

    public void Dispose()
    {
        if (_transaction != null) _transaction.Dispose();
        _context.Dispose();
    }
}