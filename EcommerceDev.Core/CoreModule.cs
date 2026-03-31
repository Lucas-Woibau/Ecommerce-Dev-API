using EcommerceDev.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceDev.Core
{
    public static class CoreModule
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddCore()
            {
                services
                    .AddDomainServices();

                return services;
            }

            private IServiceCollection AddDomainServices()
            {
                services.AddScoped<IOrderDomainService, OrderDomainService>();

                return services;
            }
        }
    }
}
