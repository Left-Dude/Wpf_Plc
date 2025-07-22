using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Interfaces.Repositories;

namespace Wpf_Plc.Infrastructure.Repositories;

public class GenericRepository<TEntity> : BaseRepository<TEntity>, IRepository<TEntity>
    where TEntity : BaseEntity
{
    public GenericRepository(PlcAppContext context) : base(context)
    {
    }
}