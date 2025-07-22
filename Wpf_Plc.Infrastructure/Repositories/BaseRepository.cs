using Microsoft.EntityFrameworkCore;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Interfaces.Repositories;

namespace Wpf_Plc.Infrastructure.Repositories;

public abstract class BaseRepository<T>(PlcAppContext context) : IRepository<T> where T : BaseEntity
{
    internal readonly PlcAppContext Context = context;

    public void AddEntity(T entity)
    {
        Context.Set<T>().Add(entity);
    }

    public virtual void DeleteEntity(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public virtual void UpdateEntity(T entity)
    {
        Context.Set<T>().Update(entity);
    }

    public virtual async Task<ICollection<T>> GetAllEntitiesAsync()
    {
        return await Context.Set<T>().AsNoTracking().ToListAsync();
    }

    public virtual async Task<T?> GetEntityById(Guid id)
    {
        return await Context.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}