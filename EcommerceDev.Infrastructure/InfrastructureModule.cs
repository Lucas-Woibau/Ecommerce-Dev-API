using Azure.Storage.Blobs;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Caching;
using EcommerceDev.Infrastructure.Messaging;
using EcommerceDev.Infrastructure.Messaging.Consumers;
using EcommerceDev.Infrastructure.Persistence;
using EcommerceDev.Infrastructure.Persistence.Repositories;
using EcommerceDev.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceDev.Infrastructure
{
    public static class InfrastructureModule
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddInfrastructure(IConfiguration configuration)
            {
                services
                    .AddData(configuration)
                    .AddRepositores()
                    .AddMessaging(configuration)
                    .AddStorage(configuration)
                    .AddCaching(configuration);

                return services;

            }

            private IServiceCollection AddData(IConfiguration configuration)
            {
                //services
                //    .AddDbContext<EcommerceDbContext>(options =>
                //    options.UseInMemoryDatabase("EcommerceDb"));

                services
                    .AddDbContext<EcommerceDbContext> (options 
                        => options.UseNpgsql(configuration.GetConnectionString("ECommerceDevDb")));

                return services;
            }

            private IServiceCollection AddRepositores()
            {
                services
                    .AddScoped<ICustomerRepository, CustomerRepository>()
                    .AddScoped<IOrderRepository, OrderRepository>()
                    .AddScoped<IProductCategoryRepository, ProductCategoryRepository>()
                    .AddScoped<IProductRepository, ProductRepository>();

                return services;
            }

            private IServiceCollection AddMessaging(IConfiguration configuration)
            {
                var rabbitMqSettings = new RabbitMqSettings();

                configuration.GetSection("RabbitMQ").Bind(rabbitMqSettings);

                services.AddSingleton(rabbitMqSettings);

                services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();

                services.AddHostedService<OrderCreatedEventConsumer>();

                return services;
            }

            private IServiceCollection AddStorage(IConfiguration configuration)
            {
                var connectionString = configuration.GetConnectionString("StorageAccount");

                var blobServiceClient = new BlobServiceClient(connectionString);

                services.AddSingleton(blobServiceClient);

                services.AddScoped<IStorageService, BlobStorageService>();

                return services;
            }

            private IServiceCollection AddCaching(IConfiguration configuration)
            {
                var provider = configuration.GetValue<string>("Caching:Provider");

                if (provider == "Redis")
                {
                    var connectionString = configuration.GetValue<string>("Caching:Redis:ConnectionString");
                    var instanceName = configuration.GetValue<string>("Caching:Redis:InstanceName");

                    services.AddStackExchangeRedisCache(o =>
                    {
                        o.Configuration = connectionString;
                        o.InstanceName = instanceName;
                    });

                    services.AddScoped<ICacheService, RedisCacheService>();
                }
                else
                {
                    services.AddMemoryCache();
                    services.AddScoped<ICacheService, MemoryCacheService>();
                }

                return services;
            }
        }
    }
}

