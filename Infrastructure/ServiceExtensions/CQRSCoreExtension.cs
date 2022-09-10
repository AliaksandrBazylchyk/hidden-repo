using Commands;
using Core.Repository;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Queries;
using System.Reflection;

namespace ServiceExtensions
{
    public static class CQRSCoreExtension
    {
        public static IServiceCollection AddCQRSCore(this IServiceCollection services)
        {
            
            return services;
        }

        public static IApplicationBuilder UseCQRSCore(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
