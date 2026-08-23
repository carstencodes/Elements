// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using HedgeCraft.Elements.Extensions.DependencyInjection.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a scoped factory for creating <typeparamref name="TService"/> instances with 1 argument.
    /// </summary>
    /// <typeparam name="TService">The service type to construct.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="factory">The factory delegate used to construct the service.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection RegisterScopedFactory<TService, T1>(this IServiceCollection services, Func<IServiceProvider, T1, TService> factory)
        where TService : notnull
        where T1 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1>, ServiceFactory<TService, T1>>();
    }

    /// <summary>
    /// Registers a scoped factory for creating <typeparamref name="TService"/> instances with 2 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to construct.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="factory">The factory delegate used to construct the service.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection RegisterScopedFactory<TService, T1, T2>(this IServiceCollection services, Func<IServiceProvider, T1, T2, TService> factory)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2>, ServiceFactory<TService, T1, T2>>();
    }

    /// <summary>
    /// Registers a scoped factory for creating <typeparamref name="TService"/> instances with 3 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to construct.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="factory">The factory delegate used to construct the service.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection RegisterScopedFactory<TService, T1, T2, T3>(this IServiceCollection services, Func<IServiceProvider, T1, T2, T3, TService> factory)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2, T3>, ServiceFactory<TService, T1, T2, T3>>();
    }

    /// <summary>
    /// Registers a scoped factory for creating <typeparamref name="TService"/> instances with 4 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to construct.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="factory">The factory delegate used to construct the service.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection RegisterScopedFactory<TService, T1, T2, T3, T4>(this IServiceCollection services, Func<IServiceProvider, T1, T2, T3, T4, TService> factory)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2, T3, T4>, ServiceFactory<TService, T1, T2, T3, T4>>();
    }

    /// <summary>
    /// Registers a scoped factory for creating <typeparamref name="TService"/> instances with 5 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to construct.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="factory">The factory delegate used to construct the service.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection RegisterScopedFactory<TService, T1, T2, T3, T4, T5>(this IServiceCollection services, Func<IServiceProvider, T1, T2, T3, T4, T5, TService> factory)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2, T3, T4, T5>, ServiceFactory<TService, T1, T2, T3, T4, T5>>();
    }

    /// <summary>
    /// Registers a scoped factory for creating <typeparamref name="TService"/> instances with 6 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to construct.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="factory">The factory delegate used to construct the service.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection RegisterScopedFactory<TService, T1, T2, T3, T4, T5, T6>(this IServiceCollection services, Func<IServiceProvider, T1, T2, T3, T4, T5, T6, TService> factory)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2, T3, T4, T5, T6>, ServiceFactory<TService, T1, T2, T3, T4, T5, T6>>();
    }

    /// <summary>
    /// Registers a scoped factory for creating <typeparamref name="TService"/> instances with 7 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to construct.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="factory">The factory delegate used to construct the service.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection RegisterScopedFactory<TService, T1, T2, T3, T4, T5, T6, T7>(
        this IServiceCollection services, Func<IServiceProvider, T1, T2, T3, T4, T5, T6, T7, TService> factory)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7>, ServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7>>();
    }
}
