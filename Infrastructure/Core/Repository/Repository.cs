using Core.Exceptions;
using DatabaseExtensions;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Core.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly DatabaseContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(
            DatabaseContext context
        )
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);

            return entity;
        }

        public async Task<TEntity> DeleteAsync(Guid id)
        {            
            var entity = await DbSet.FindAsync(id) ??
                         throw new NotFoundException("Entity with this ID doesn't exist");

            DbSet.Remove(entity);

            return entity;
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id) ?? throw new NotFoundException("Entity with this ID doesn't exist");
        }

       
    }
}
