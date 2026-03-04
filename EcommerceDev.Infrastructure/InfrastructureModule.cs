using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Persistence;
using EcommerceDev.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceDev.Infrastructure
{
    public static class InfrastructureModule
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddInfrastructure()
            {
                services
                    .AddData()
                    .AddRepositores();

                return services;

            }

            private IServiceCollection AddData()
            {
                services
                    .AddDbContext<EcommerceDbContext>(options =>
                    options.UseInMemoryDatabase("EcommerceDb"));

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
        }
    }
}

