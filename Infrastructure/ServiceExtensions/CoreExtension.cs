using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

using Commands;
using Queries;

using Core.Aggregate;
using Core.Outbox;
using Core.Repository;

namespace ServiceExtensions
{
    public static class CoreExtension
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IQueryBus, QueryBus>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IOutboxStore<>), typeof(OutboxStore<>));
            services.AddScoped(typeof(IAggregate<,>), typeof(Aggregate<,>));

            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }

        public static IApplicationBuilder UseCore(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
