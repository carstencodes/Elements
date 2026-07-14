using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    // based upon https://blog.greatrexpectations.com/2018/11/07/composite-pattern-in-net-core-with-dependency-injection

    public static void AddComposite<TInterface, TComposite>(this IServiceCollection services,
        Func<IServiceProvider, TComposite> objectFactory)
        where TInterface : notnull
        where TComposite : class, TInterface
    {
        TComposite CreateFromServiceProvider(IServiceProvider serviceProvider, object?[]? _)
        {
            return objectFactory(serviceProvider);
        }
        
        services.AddComposite<TInterface, TComposite>(CreateFromServiceProvider);
    }
    
    public static void AddComposite<TInterface, TComposite>(this IServiceCollection services, ObjectFactory<TComposite> objectFactory)
        where TInterface : notnull
        where TComposite : class, TInterface
    {
        IReadOnlyCollection<ServiceDescriptor>
            replacedDescriptors = services.GetExistingServiceDescriptors<TInterface>();
        foreach (ServiceDescriptor descriptor in replacedDescriptors)
        {
            services.Remove(descriptor);
        }

        ServiceLifetime selectedLifetime = ServiceLifetime.Singleton;
        if (replacedDescriptors.Count > 0)
        {
            selectedLifetime = replacedDescriptors.Max(d => d.Lifetime);
        }

        Func<IServiceProvider, IReadOnlyCollection<ServiceDescriptor>, object[]> createReplacedInstances =
            CreateAnonymousInstancesFromServiceDescriptors<TInterface>; 
        
        ServiceDescriptor newServiceDescriptor = ServiceDescriptor.Describe(
            typeof(TInterface),
            sp => objectFactory(
                sp,
                createReplacedInstances(sp, replacedDescriptors)
                ),
            selectedLifetime);
        
        services.Add(newServiceDescriptor);
    }

    public static void AddComposite<TInterface, TComposite>(this IServiceCollection services)
        where TInterface : notnull
        where TComposite : class, TInterface
    {
        ObjectFactory<TComposite> objectFactory = ActivatorUtilities.CreateFactory<TComposite>(
            new[] { typeof(IEnumerable<TInterface>) });

        services.AddComposite<TInterface, TComposite>(objectFactory);
    }

    private static IReadOnlyCollection<ServiceDescriptor> GetExistingServiceDescriptors<TInterface>(
        this IServiceCollection services)
    {
        return services.Where(s => s.ServiceType == typeof(TInterface)).ToArray();
    }
    
    private static object[] CreateAnonymousInstancesFromServiceDescriptors<TInterface>(IServiceProvider serviceProvider, IReadOnlyCollection<ServiceDescriptor> serviceDescriptors)
        where TInterface: notnull
    {
        object InstantiateFromServiceDescriptor(ServiceDescriptor descriptor)
        {
            object instance = serviceProvider.CreateInstance(descriptor);
            if (instance is not TInterface concreteInstance)
            {
                throw new InvalidOperationException($"Service Descriptor does not provide an instance of {typeof(TInterface).FullName}");
            }

            return concreteInstance;
        }
        
        return serviceDescriptors.Select(InstantiateFromServiceDescriptor).ToArray();
    }
}