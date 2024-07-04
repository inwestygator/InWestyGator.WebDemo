using InWestyGator.WebDemo.Core.Contracts;
using InWestyGator.WebDemo.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace InWestyGator.WebDemo.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebPostgreDataAccess(this IServiceCollection services, string connectionString, ILoggerFactory loggerFactory = null)
        {
            services.AddScoped<IPackRepository, PackRepository>();

            // add the mock here for now
            services.AddScoped<IUserRepository, MockUserRepository>();

            services.AddDbContext<WebDbContext>(options =>
            {
                options.UseNpgsql(connectionString, o => {
                    // TODO: do we want to configure these with Action<> patern?
                    o.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                    o.MaxBatchSize(50);
                });
                // TODO: check performance impact, this will automatically load the navigation properties
                //options.UseLazyLoadingProxies();

                if (loggerFactory != null)
                {
                    options.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();
                }
            });

            return services;
        }
    }
}
