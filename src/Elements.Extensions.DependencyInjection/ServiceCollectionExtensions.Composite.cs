// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for registering services in an <see cref="IServiceCollection"/>.
/// </summary>
public static partial class ServiceCollectionExtensions
{
    // based upon https://blog.greatrexpectations.com/2018/11/07/composite-pattern-in-net-core-with-dependency-injection

    /// <summary>
    /// Registers a composite service implementation for the specified service interface using a service provider factory.
    /// </summary>
    /// <typeparam name="TInterface">The service interface type.</typeparam>
    /// <typeparam name="TComposite">The composite implementation type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="objectFactory">The factory function to instantiate the composite.</param>
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

    /// <summary>
    /// Registers a composite service implementation for the specified service interface using an <see cref="ObjectFactory{T}"/>.
    /// </summary>
    /// <typeparam name="TInterface">The service interface type.</typeparam>
    /// <typeparam name="TComposite">The composite implementation type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="objectFactory">The object factory used to instantiate the composite with replaced service instances.</param>
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

    /// <summary>
    /// Registers a composite service implementation for the specified service interface using activator utilities.
    /// </summary>
    /// <typeparam name="TInterface">The service interface type.</typeparam>
    /// <typeparam name="TComposite">The composite implementation type.</typeparam>
    /// <param name="services">The service collection.</param>
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
        where TInterface : notnull
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
