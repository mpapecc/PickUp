using PickUp.Common.Domain.BaseModels;

namespace PickUp.Common.Application
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        Task CommitAsync();
        Task DeleteAsync(T entity);
        IQueryable<T> Query();
    }
}
