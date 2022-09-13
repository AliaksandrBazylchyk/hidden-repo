using Microsoft.EntityFrameworkCore;

namespace Models.Entities
{
    public class MovieContext
    {
        public static void Build(ModelBuilder builder)
        {
            builder.Entity<Movie>(b =>
            {
                b.Property(p => p.Id);

                b.Property(p => p.Title);

                b.Property(p => p.Year);

                b.HasKey(k => k.Id);
            });
        }
    }
}
