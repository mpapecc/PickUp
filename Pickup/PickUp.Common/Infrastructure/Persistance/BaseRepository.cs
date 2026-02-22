using PickUp.Common.Application;
using PickUp.Common.Domain.BaseModels;
using PickUp.Common.Infrastructure.Database;

namespace PickUp.Common.Infrastructure.Persistance
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly BaseDbContext context;

        public BaseRepository(BaseDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(T entity)
        {
            entity.Id = Guid.CreateVersion7();

            await context.Set<T>().AddAsync(entity);
        }

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() => context.Set<T>().Remove(entity));
        }

        public IQueryable<T> Query()
        {
            return context.Set<T>();
        }
    }
}
