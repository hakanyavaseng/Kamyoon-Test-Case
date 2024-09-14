using Microsoft.EntityFrameworkCore.Storage;

namespace ProductManagement.Core.Interfaces.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    ///     Commits the changes to the database asynchronously
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CommitAsync();

    /// <summary>
    ///     Commits the changes to the database.
    /// </summary>
    void Commit();

    /// <summary>
    ///     Rolls back the transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RollbackAsync();

    /// <summary>
    ///     Rolls back the transaction.
    /// </summary>
    void Rollback();

    /// <summary>
    ///     Begins a transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the transaction operation.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Begins a transaction.
    /// </summary>
    /// <returns>The transaction object.</returns>
    IDbContextTransaction BeginTransaction();

    /*
    /// <summary>
    /// Retrieves the repository for a specific entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The repository for the specified entity.</returns>
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    */
    /// <summary>
    ///     Saves changes made to the context asynchronously.
    /// </summary>
    /// <returns>A task representing the save operation.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Saves changes made to the context.
    /// </summary>
    /// <returns>The number of affected entities.</returns>
    int SaveChanges();
}