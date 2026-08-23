// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HedgeCraft.Elements.Extensions.DependencyInjection.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.DependencyInjection;

/// <summary>
/// Provides internal extension methods for <see cref="IServiceProvider"/>.
/// </summary>
internal static class ServiceProviderExtensions
{
    // based upon https://greatrexpectations.com/2018/10/25/decorators-in-net-core-with-dependency-injection
    // and https://blog.greatrexpectations.com/2018/11/07/composite-pattern-in-net-core-with-dependency-injection

    /// <summary>
    /// Creates an instance of a service described by the specified <see cref="ServiceDescriptor"/>.
    /// </summary>
    /// <param name="services">The service provider.</param>
    /// <param name="descriptor">The service descriptor describing the service to create.</param>
    /// <returns>The created service instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service descriptor contains no valid instance, factory, or implementation type.</exception>
    internal static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
    {
        if (descriptor.ImplementationInstance is not null)
        {
            return descriptor.ImplementationInstance;
        }

        if (descriptor.ImplementationFactory is not null)
        {
            return descriptor.ImplementationFactory(services);
        }

        if (descriptor.ImplementationType is not null)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType);
        }

        throw new InvalidOperationException("Specified service descriptor does not provide required information.");
    }

    /// <summary>
    /// Finds a registered factory function to create instances of <typeparamref name="TService"/> with 1 argument.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A factory delegate producing <typeparamref name="TService"/> instances.</returns>
    internal static Func<T1, TService> FindRequiredServiceFactory<TService, T1>(this IServiceProvider serviceProvider)
    {
        Func<T1, TService>? factoryFunction = serviceProvider.GetService<Func<T1, TService>>();
        if (factoryFunction is not null)
        {
            return factoryFunction;
        }

        IServiceFactory<TService, T1>? factoryClass = serviceProvider.GetService<IServiceFactory<TService, T1>>();
        if (factoryClass is not null)
        {
            return factoryClass.CreateInstance;
        }

        return (_) => NoValidProviderRegistered<TService>(typeof(T1));
    }

    /// <summary>
    /// Finds a registered factory function to create instances of <typeparamref name="TService"/> with 2 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A factory delegate producing <typeparamref name="TService"/> instances.</returns>
    internal static Func<T1, T2, TService> FindRequiredServiceFactory<TService, T1, T2>(this IServiceProvider serviceProvider)
    {
        Func<T1, T2, TService>? factoryFunction = serviceProvider.GetService<Func<T1, T2, TService>>();
        if (factoryFunction is not null)
        {
            return factoryFunction;
        }

        IServiceFactory<TService, T1, T2>? factoryClass = serviceProvider.GetService<IServiceFactory<TService, T1, T2>>();
        if (factoryClass is not null)
        {
            return factoryClass.CreateInstance;
        }

        return (_, _) => NoValidProviderRegistered<TService>(typeof(T1), typeof(T2));
    }

    /// <summary>
    /// Finds a registered factory function to create instances of <typeparamref name="TService"/> with 3 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A factory delegate producing <typeparamref name="TService"/> instances.</returns>
    internal static Func<T1, T2, T3, TService> FindRequiredServiceFactory<TService, T1, T2, T3>(this IServiceProvider serviceProvider)
    {
        Func<T1, T2, T3, TService>? factoryFunction = serviceProvider.GetService<Func<T1, T2, T3, TService>>();
        if (factoryFunction is not null)
        {
            return factoryFunction;
        }

        IServiceFactory<TService, T1, T2, T3>? factoryClass = serviceProvider.GetService<IServiceFactory<TService, T1, T2, T3>>();
        if (factoryClass is not null)
        {
            return factoryClass.CreateInstance;
        }

        return (_, _, _) => NoValidProviderRegistered<TService>(typeof(T1), typeof(T2), typeof(T3));
    }

    /// <summary>
    /// Finds a registered factory function to create instances of <typeparamref name="TService"/> with 4 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A factory delegate producing <typeparamref name="TService"/> instances.</returns>
    internal static Func<T1, T2, T3, T4, TService> FindRequiredServiceFactory<TService, T1, T2, T3, T4>(this IServiceProvider serviceProvider)
    {
        Func<T1, T2, T3, T4, TService>? factoryFunction = serviceProvider.GetService<Func<T1, T2, T3, T4, TService>>();
        if (factoryFunction is not null)
        {
            return factoryFunction;
        }

        IServiceFactory<TService, T1, T2, T3, T4>? factoryClass = serviceProvider.GetService<IServiceFactory<TService, T1, T2, T3, T4>>();
        if (factoryClass is not null)
        {
            return factoryClass.CreateInstance;
        }

        return (_, _, _, _) => NoValidProviderRegistered<TService>(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
    }

    /// <summary>
    /// Finds a registered factory function to create instances of <typeparamref name="TService"/> with 5 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A factory delegate producing <typeparamref name="TService"/> instances.</returns>
    internal static Func<T1, T2, T3, T4, T5, TService> FindRequiredServiceFactory<TService, T1, T2, T3, T4, T5>(this IServiceProvider serviceProvider)
    {
        Func<T1, T2, T3, T4, T5, TService>? factoryFunction = serviceProvider.GetService<Func<T1, T2, T3, T4, T5, TService>>();
        if (factoryFunction is not null)
        {
            return factoryFunction;
        }

        IServiceFactory<TService, T1, T2, T3, T4, T5>? factoryClass = serviceProvider.GetService<IServiceFactory<TService, T1, T2, T3, T4, T5>>();
        if (factoryClass is not null)
        {
            return factoryClass.CreateInstance;
        }

        return (_, _, _, _, _) => NoValidProviderRegistered<TService>(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
    }

    /// <summary>
    /// Finds a registered factory function to create instances of <typeparamref name="TService"/> with 6 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A factory delegate producing <typeparamref name="TService"/> instances.</returns>
    internal static Func<T1, T2, T3, T4, T5, T6, TService> FindRequiredServiceFactory<TService, T1, T2, T3, T4, T5, T6>(this IServiceProvider serviceProvider)
    {
        Func<T1, T2, T3, T4, T5, T6, TService>? factoryFunction = serviceProvider.GetService<Func<T1, T2, T3, T4, T5, T6, TService>>();
        if (factoryFunction is not null)
        {
            return factoryFunction;
        }

        IServiceFactory<TService, T1, T2, T3, T4, T5, T6>? factoryClass = serviceProvider.GetService<IServiceFactory<TService, T1, T2, T3, T4, T5, T6>>();
        if (factoryClass is not null)
        {
            return factoryClass.CreateInstance;
        }

        return (_, _, _, _, _, _) => NoValidProviderRegistered<TService>(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
    }

    /// <summary>
    /// Finds a registered factory function to create instances of <typeparamref name="TService"/> with 7 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument.</typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A factory delegate producing <typeparamref name="TService"/> instances.</returns>
    internal static Func<T1, T2, T3, T4, T5, T6, T7, TService> FindRequiredServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7>(this IServiceProvider serviceProvider)
    {
        Func<T1, T2, T3, T4, T5, T6, T7, TService>? factoryFunction = serviceProvider.GetService<Func<T1, T2, T3, T4, T5, T6, T7, TService>>();
        if (factoryFunction is not null)
        {
            return factoryFunction;
        }

        IServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7>? factoryClass = serviceProvider.GetService<IServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7>>();
        if (factoryClass is not null)
        {
            return factoryClass.CreateInstance;
        }

        return (_, _, _, _, _, _, _) => NoValidProviderRegistered<TService>(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
    }

    [DoesNotReturn]
    private static TService NoValidProviderRegistered<TService>(params Type[] types)
    {
        string?[] typeNames = types.Select(t => t.FullName).ToArray();
        string message =
            $"Service provider does not return a factory for {typeof(TService).FullName} accepting {string.Join(", ", typeNames)} as arguments.";
        throw new InvalidOperationException(message);
    }
}
