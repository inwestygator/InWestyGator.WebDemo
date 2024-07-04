using InWestyGator.WebDemo.Business.Services;
using InWestyGator.WebDemo.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace InWestyGator.WebDemo.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<IPackService, PackService>();

            return services;
        }
    }
}
