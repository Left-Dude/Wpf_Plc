using Wpf_Plc.Domain.Entities;

namespace Wpf_Plc.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity>? GetRepository<TEntity>() where TEntity : BaseEntity;
    Task<int> CommitAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackAsync();
    void Dispose();
}