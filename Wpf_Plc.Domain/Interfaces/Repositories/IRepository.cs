using Wpf_Plc.Domain.Entities;

namespace Wpf_Plc.Domain.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    void AddEntity(T entity);
    void DeleteEntity(T entity);
    void UpdateEntity(T entity);
    Task<List<T>> GetAllEntitiesAsync();
    Task<T?> GetEntityById(Guid id);
}