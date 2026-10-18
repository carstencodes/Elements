// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HedgeCraft.Elements.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    // based upon https://greatrexpectations.com/2018/10/25/decorators-in-net-core-with-dependency-injection

    /// <summary>
    /// Decorates an existing service registration of type <typeparamref name="TInterface"/> with a decorator instance created by a factory.
    /// </summary>
    /// <typeparam name="TInterface">The service interface type being decorated.</typeparam>
    /// <typeparam name="TDecorator">The decorator type implementing <typeparamref name="TInterface"/>.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="objectFactory">The factory creating the decorator instance.</param>
    /// <exception cref="InvalidOperationException">Thrown when <typeparamref name="TInterface"/> has not been registered in <paramref name="services"/>.</exception>
    public static void Decorate<TInterface, TDecorator>(
        this IServiceCollection services,
        Func<IServiceProvider, TDecorator> objectFactory)
        where TInterface : notnull
        where TDecorator : class, TInterface
    {
        TDecorator CreateInstanceOfDecorator(IServiceProvider serviceProvider, object?[]? _)
        {
            return objectFactory(serviceProvider);
        }

        services.Decorate<TInterface, TDecorator>(CreateInstanceOfDecorator);
    }

    /// <summary>
    /// Decorates an existing service registration of type <typeparamref name="TInterface"/> with a decorator instance using an <see cref="ObjectFactory{T}"/>.
    /// </summary>
    /// <typeparam name="TInterface">The service interface type being decorated.</typeparam>
    /// <typeparam name="TDecorator">The decorator type implementing <typeparamref name="TInterface"/>.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="objectFactory">The object factory creating the decorator instance.</param>
    /// <exception cref="InvalidOperationException">Thrown when <typeparamref name="TInterface"/> has not been registered in <paramref name="services"/>.</exception>
    public static void Decorate<TInterface, TDecorator>(
        this IServiceCollection services,
        ObjectFactory<TDecorator> objectFactory)
        where TInterface : notnull
        where TDecorator : class, TInterface
    {
        ServiceDescriptor existingDescriptor = services.GetExistingServiceDescriptor<TInterface>();

        services.Replace(ServiceDescriptor.Describe(
            typeof(TInterface),
            sp => objectFactory(sp, new[]
            {
                sp.CreateInstance(existingDescriptor)
            }),
            existingDescriptor.Lifetime)
        );
    }

    /// <summary>
    /// Decorates an existing service registration of type <typeparamref name="TInterface"/> with a decorator instance created via constructor activation.
    /// </summary>
    /// <typeparam name="TInterface">The service interface type being decorated.</typeparam>
    /// <typeparam name="TDecorator">The decorator type implementing <typeparamref name="TInterface"/>.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <exception cref="InvalidOperationException">Thrown when <typeparamref name="TInterface"/> has not been registered in <paramref name="services"/>.</exception>
    public static void Decorate<TInterface, TDecorator>(this IServiceCollection services)
        where TInterface : notnull
        where TDecorator : class, TInterface
    {
        ObjectFactory<TDecorator> objectFactory = ActivatorUtilities.CreateFactory<TDecorator>(new[] { typeof(TInterface) });

        services.Decorate<TInterface, TDecorator>(objectFactory);
    }

    private static ServiceDescriptor GetExistingServiceDescriptor<TInterface>(this IServiceCollection services)
        where TInterface : notnull
    {
        if (!services.TryGetServiceDescriptor<TInterface>(out ServiceDescriptor? existingDescriptor))
        {
            throw new InvalidOperationException($"{typeof(TInterface).Name} is not registered");
        }

        return existingDescriptor;
    }

    private static bool TryGetServiceDescriptor<TInterface>(this IServiceCollection services, [NotNullWhen(true)] out ServiceDescriptor? descriptor)
    {
        ServiceDescriptor? wrappedDescriptor = services.FirstOrDefault(
            s => s.ServiceType == typeof(TInterface));

        if (wrappedDescriptor is null)
        {
            descriptor = default;
            return false;
        }

        descriptor = wrappedDescriptor;
        return true;
    }
}
