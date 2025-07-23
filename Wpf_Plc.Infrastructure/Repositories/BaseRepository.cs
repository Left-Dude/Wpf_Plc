using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Interfaces.Repositories;

namespace Wpf_Plc.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly PlcAppContext Context;

        protected BaseRepository(PlcAppContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

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

        public virtual async Task<List<T>> GetAllEntitiesAsync()
        {
            return await Context.Set<T>()
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public virtual async Task<T?> GetEntityById(Guid id)
        {
            return await Context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id)
                .ConfigureAwait(false);
        }
    }
}