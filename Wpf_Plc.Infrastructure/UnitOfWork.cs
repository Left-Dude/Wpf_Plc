using Microsoft.EntityFrameworkCore.Storage;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Interfaces.Repositories;
using Wpf_Plc.Infrastructure.Repositories;

namespace Wpf_Plc.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly PlcAppContext _context;
    private IDbContextTransaction? _transaction;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(PlcAppContext context)
    {
        _context = context;
    }

    public IRepository<TEntity>? GetRepository<TEntity>() where TEntity : BaseEntity
    {
        var type = typeof(TEntity);
        
        if(_repositories.TryGetValue(type, out var repository))
            return repository as IRepository<TEntity>;
        
        var repositoryType = typeof(GenericRepository<>).MakeGenericType(type);
        var newRepository = Activator.CreateInstance(repositoryType, _context);
        
        _repositories.Add(type, newRepository);
        return newRepository as IRepository<TEntity>;
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction == null)
            _transaction = await _context.Database.BeginTransactionAsync()
                .ConfigureAwait(false);
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            // Коммитим и сразу же очищаем транзакцию
            await _transaction.CommitAsync().ConfigureAwait(false);
            await _transaction.DisposeAsync().ConfigureAwait(false);
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            try
            {
                await _transaction.RollbackAsync().ConfigureAwait(false);
            }
            catch
            {
                // игнорируем, если транзакция уже была завершена
            }
            finally
            {
                await _transaction.DisposeAsync().ConfigureAwait(false);
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}