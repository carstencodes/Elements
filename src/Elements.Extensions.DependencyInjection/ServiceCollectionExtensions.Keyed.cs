using HedgeCraft.Elements.Extensions.DependencyInjection.KeyedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HedgeCraft.Elements.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddKeyesForKeyedServices<TService>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton) 
        where TService : notnull
    {
        services.Add(new ServiceDescriptor(
            typeof(IKeyedServiceCollection<TService>),
            typeof(KeyedServiceCollection<TService>),
            lifetime));
        return services.AddServiceKeyCollection<TService>(lifetime);
    }

    private static IServiceCollection AddServiceKeyCollection<TService>(this IServiceCollection services, ServiceLifetime lifetime)
        where TService: notnull
    {
        Func<IReadOnlyCollection<KeyValuePair<object, Type>>> serviceKeyFactory = services
            .Where(descriptor =>
                descriptor.IsKeyedService && descriptor.ServiceType.IsAssignableTo(typeof(TService)))
            .Where(item => item.ServiceKey is not null)
            .Select(descriptor => new KeyValuePair<object, Type>(descriptor.ServiceKey!, 
                descriptor.KeyedImplementationType 
                ?? descriptor.KeyedImplementationInstance?.GetType() 
                ?? descriptor.KeyedImplementationFactory?.GetType().GetGenericArguments()[2]
                ?? typeof(void)))
            .DistinctBy(kvp => kvp.Key).ToArray;
        
        services.Add(new ServiceDescriptor(
            typeof(IServiceKeyCollection<TService>),
            _ => new ServiceKeyCollection<TService>(serviceKeyFactory()),
            lifetime
            ));

        return services;
    }
}