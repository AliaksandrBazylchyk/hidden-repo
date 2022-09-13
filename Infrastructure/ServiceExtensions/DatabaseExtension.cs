using DatabaseExtensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceExtensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddSQLDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));

            return services;
        }

        public static IApplicationBuilder UseSQLDatabase(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
