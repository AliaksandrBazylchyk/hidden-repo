using Models.OutboxEntities;

namespace Core.Outbox
{
    public interface IOutboxStore<TOutboxEntity> 
        where TOutboxEntity : BaseOutbox
    {
        Task<TOutboxEntity> StoreCreateAsync(object o);
        Task<TOutboxEntity> CommitRecordAsync(Guid id);
        Task<TOutboxEntity> GetFirstRecordAsync();
    }
}
