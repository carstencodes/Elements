// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for <see cref="IServiceScope"/> to create dynamic service instances using registered factories.
/// </summary>
public static class ServiceScopeExtensions
{
    /// <summary>
    /// Creates a new service instance of <typeparamref name="TService"/> from the service scope with 1 argument.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <param name="scope">The service scope.</param>
    /// <param name="arg1">The first argument.</param>
    /// <returns>A new instance of <typeparamref name="TService"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no matching service factory is registered.</exception>
    public static TService CreateDynamicInstance<TService, T1>(this IServiceScope scope, T1 arg1)
        where TService : notnull
        where T1 : notnull
    {
        Func<T1, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1>();
        return serviceFactory(arg1);
    }

    /// <summary>
    /// Creates a new service instance of <typeparamref name="TService"/> from the service scope with 2 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <param name="scope">The service scope.</param>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <returns>A new instance of <typeparamref name="TService"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no matching service factory is registered.</exception>
    public static TService CreateDynamicInstance<TService, T1, T2>(this IServiceScope scope, T1 arg1, T2 arg2)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
    {
        Func<T1, T2, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2>();
        return serviceFactory(arg1, arg2);
    }

    /// <summary>
    /// Creates a new service instance of <typeparamref name="TService"/> from the service scope with 3 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <param name="scope">The service scope.</param>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <returns>A new instance of <typeparamref name="TService"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no matching service factory is registered.</exception>
    public static TService CreateDynamicInstance<TService, T1, T2, T3>(this IServiceScope scope, T1 arg1, T2 arg2, T3 arg3)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
    {
        Func<T1, T2, T3, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2, T3>();
        return serviceFactory(arg1, arg2, arg3);
    }

    /// <summary>
    /// Creates a new service instance of <typeparamref name="TService"/> from the service scope with 4 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <param name="scope">The service scope.</param>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <returns>A new instance of <typeparamref name="TService"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no matching service factory is registered.</exception>
    public static TService CreateDynamicInstance<TService, T1, T2, T3, T4>(this IServiceScope scope, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
    {
        Func<T1, T2, T3, T4, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2, T3, T4>();
        return serviceFactory(arg1, arg2, arg3, arg4);
    }

    /// <summary>
    /// Creates a new service instance of <typeparamref name="TService"/> from the service scope with 5 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <param name="scope">The service scope.</param>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth argument.</param>
    /// <returns>A new instance of <typeparamref name="TService"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no matching service factory is registered.</exception>
    public static TService CreateDynamicInstance<TService, T1, T2, T3, T4, T5>(this IServiceScope scope, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
    {
        Func<T1, T2, T3, T4, T5, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2, T3, T4, T5>();
        return serviceFactory(arg1, arg2, arg3, arg4, arg5);
    }

    /// <summary>
    /// Creates a new service instance of <typeparamref name="TService"/> from the service scope with 6 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <param name="scope">The service scope.</param>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth argument.</param>
    /// <param name="arg6">The sixth argument.</param>
    /// <returns>A new instance of <typeparamref name="TService"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no matching service factory is registered.</exception>
    public static TService CreateDynamicInstance<TService, T1, T2, T3, T4, T5, T6>(this IServiceScope scope, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
    {
        Func<T1, T2, T3, T4, T5, T6, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2, T3, T4, T5, T6>();
        return serviceFactory(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    /// <summary>
    /// Creates a new service instance of <typeparamref name="TService"/> from the service scope with 7 arguments.
    /// </summary>
    /// <typeparam name="TService">The service type to create.</typeparam>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument.</typeparam>
    /// <param name="scope">The service scope.</param>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth argument.</param>
    /// <param name="arg6">The sixth argument.</param>
    /// <param name="arg7">The seventh argument.</param>
    /// <returns>A new instance of <typeparamref name="TService"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no matching service factory is registered.</exception>
    public static TService CreateDynamicInstance<TService, T1, T2, T3, T4, T5, T6, T7>(this IServiceScope scope, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
    {
        Func<T1, T2, T3, T4, T5, T6, T7, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7>();
        return serviceFactory(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }
}
