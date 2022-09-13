using Core.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceExtensions
{
    public static class RabbitMqExtension
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IRabbitMqCQRSHelper), typeof(RabbitMqCQRSHelper));

            return services;
        }

        public static IApplicationBuilder UseRabbitMq(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
