using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities.Common;

namespace ProductManagement.Core.Interfaces.Repositories;

public interface IRepositoryManager : IDisposable
{
    DbContext DbContext { get; }
    IReadRepository<T> GetReadRepository<T>() where T : CreationAuditedEntity<Guid>;
    IWriteRepository<T> GetWriteRepository<T>() where T : CreationAuditedEntity<Guid>;
    Task<int> SaveAsync();
    int Save();
}