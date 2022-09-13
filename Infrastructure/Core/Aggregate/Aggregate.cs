using Core.Outbox;
using Core.Repository;
using DatabaseExtensions;
using Models.Entities;
using Models.OutboxEntities;

namespace Core.Aggregate
{
    public class Aggregate<TEntity, TEntityOutbox> : IAggregate<TEntity, TEntityOutbox>
        where TEntity : BaseEntity
        where TEntityOutbox : BaseOutbox
    {
        protected readonly DatabaseContext Context;
        protected readonly IRepository<TEntity> Repository;
        protected readonly IOutboxStore<TEntityOutbox> OutboxStore;
        public Aggregate(
            DatabaseContext context,
            IRepository<TEntity> repository,
            IOutboxStore<TEntityOutbox> outboxStore
            )
        {
            Context = context;
            Repository = repository;
            OutboxStore = outboxStore;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            using var transaction = await Context.Database.BeginTransactionAsync();

            var result = await Repository.CreateAsync(entity);
            var record = await OutboxStore.StoreCreateAsync(result);

            await Context.SaveChangesAsync();

            await transaction.CommitAsync();

            return result;
        }
    }
}
