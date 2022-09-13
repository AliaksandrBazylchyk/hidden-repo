using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.OutboxEntities;

namespace DatabaseExtensions
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieOutbox> MoviesOutbox { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            MovieContext.Build(builder);
        }
    }
}
