using System.Linq.Expressions;
using ProductManagement.Domain.Entities.Common;

namespace ProductManagement.Core.Interfaces.Repositories;

public interface IWriteRepository<T> where T : CreationAuditedEntity<Guid>
{
    //Create
    Task AddAsync(T entity);

    Task AddAsync(IEnumerable<T> entities);

    //Update
    Task UpdateAsync(T entity);

    void Update(T entity);

    //Delete
    Task DeleteAsync(T entity);
    Task DeleteAsync(Expression<Func<T, bool>> predicate);
}