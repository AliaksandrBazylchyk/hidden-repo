using Models;

namespace Core.Aggregate
{
    public interface IAggregate<TEntity, TEntityOutbox>
        where TEntity : BaseEntity
        where TEntityOutbox : BaseOutbox
    {
        Task<TEntity> CreateAsync(TEntity entity);
        //Task<TEntity> DeleteAsync(Guid id);

    }
}
