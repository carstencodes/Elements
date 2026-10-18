// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Diagnostics.CodeAnalysis;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.Factories;

/// <summary>
/// Provides a service factory implementation for creating instances of <typeparamref name="TService"/> with six arguments.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
/// <typeparam name="T1">The first argument type.</typeparam>
/// <typeparam name="T2">The second argument type.</typeparam>
/// <typeparam name="T3">The third argument type.</typeparam>
/// <typeparam name="T4">The fourth argument type.</typeparam>
/// <typeparam name="T5">The fifth argument type.</typeparam>
/// <typeparam name="T6">The sixth argument type.</typeparam>
[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Internal factory of the IServiceFactory family, instantiated via dependency injection.")]
internal sealed class ServiceFactory<TService, T1, T2, T3, T4, T5, T6> : IServiceFactory<TService, T1, T2, T3, T4, T5, T6>
{
    private readonly IServiceProvider provider;
    private readonly Func<IServiceProvider, T1, T2, T3, T4, T5, T6, TService> factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceFactory{TService,T1,T2,T3,T4,T5,T6}"/> class.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <param name="factory">The factory function to instantiate the service.</param>
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, T3, T4, T5, T6, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    /// <inheritdoc />
    public TService CreateInstance(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return this.factory(this.provider, arg1, arg2, arg3, arg4, arg5, arg6);
    }
}
