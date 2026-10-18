// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Diagnostics.CodeAnalysis;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.Factories;

/// <summary>
/// Provides a service factory implementation for creating instances of <typeparamref name="TService"/> with two arguments.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
/// <typeparam name="T1">The first argument type.</typeparam>
/// <typeparam name="T2">The second argument type.</typeparam>
internal sealed class ServiceFactory<TService, T1, T2> : IServiceFactory<TService, T1, T2>
{
    private readonly IServiceProvider provider;
    private readonly Func<IServiceProvider, T1, T2, TService> factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceFactory{TService,T1,T2}"/> class.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <param name="factory">The factory function to instantiate the service.</param>
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    /// <inheritdoc />
    public TService CreateInstance(T1 arg1, T2 arg2)
    {
        return this.factory(this.provider, arg1, arg2);
    }
}
