using EcommerceDev.Application.Common;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceDev.Application
{
    public static class ApplicationModule
    {

        extension(IServiceCollection services)
        {
            public IServiceCollection AddApplication()
            {
                services.AddHandlers();

                return services;
            }

            private IServiceCollection AddHandlers()
            {
                services.AddSingleton<IMediator, Mediator>();

                services.Scan(s =>
                    s.FromAssemblies(typeof(ApplicationModule).Assembly)
                        .AddClasses(c => c.AssignableTo(typeof(IHandler<,>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
                );

                return services;
            }
        }
    }
}
