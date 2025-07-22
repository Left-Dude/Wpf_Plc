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
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null) await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }
}