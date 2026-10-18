// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Diagnostics.CodeAnalysis;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.Factories;

/// <summary>
/// Provides a service factory implementation for creating instances of <typeparamref name="TService"/> with one argument.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
/// <typeparam name="T">The first argument type.</typeparam>
internal sealed class ServiceFactory<TService, T> : IServiceFactory<TService, T>
{
    private readonly IServiceProvider provider;
    private readonly Func<IServiceProvider, T, TService> factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceFactory{TService, T1}"/> class.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <param name="factory">The factory function to instantiate the service.</param>
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    /// <inheritdoc />
    public TService CreateInstance(T arg)
    {
        return this.factory(this.provider, arg);
    }
}
