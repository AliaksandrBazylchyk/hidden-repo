using Models.Entities;

namespace Core.Repository
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(Guid id);
    }
}
