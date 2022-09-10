using Core.Exceptions;
using DatabaseExtensions;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;

namespace Core.Outbox
{
    public class OutboxStore<TOutboxEntity> : IOutboxStore<TOutboxEntity>
        where TOutboxEntity : BaseOutbox, new()
    {
        protected readonly DatabaseContext Context;
        protected readonly DbSet<TOutboxEntity> DbSet;

        public OutboxStore(
            DatabaseContext context
            )
        {
            Context = context;
            DbSet = context.Set<TOutboxEntity>(); ;
        }

        public async Task<TOutboxEntity> StoreCreateAsync(object o)
        {
            var jsonObject = await SerializeAsync(o);

            var outboxRecord = new TOutboxEntity
            {
                Id = Guid.NewGuid(),
                Method = Enums.MethodType.CREATE,
                Object = jsonObject
            };

            await DbSet.AddAsync(outboxRecord);

            return outboxRecord;
        }

        public async Task<TOutboxEntity> CommitRecordAsync(Guid id)
        {
            var record = await DbSet.FindAsync(id) ??
                         throw new NotFoundException("Record with this ID doesn't exist");

            record.IsCompleted = true;

            DbSet.Update(record);

            return record;
        }

        public async Task<TOutboxEntity> GetFirstRecordAsync()
        {
            var record = await DbSet.OrderByDescending(x => x.CreatedDateTimeUtc).FirstOrDefaultAsync(x => x.IsCompleted == false) ??
                throw new NotFoundException("Record with this ID doesn't exist");

            return record;
        }
        private static async Task<string> SerializeAsync(object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
