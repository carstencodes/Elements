// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.Factories;

/// <summary>
/// Provides a service factory implementation for creating instances of <typeparamref name="TService"/> with one argument.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
/// <typeparam name="T1">The first argument type.</typeparam>
internal sealed class ServiceFactory<TService, T1> : IServiceFactory<TService, T1>
{
    private readonly IServiceProvider provider;
    private readonly Func<IServiceProvider, T1, TService> factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceFactory{TService, T1}"/> class.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <param name="factory">The factory function to instantiate the service.</param>
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    /// <inheritdoc />
    public TService CreateInstance(T1 arg1)
    {
        return this.factory(this.provider, arg1);
    }
}

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
    /// Initializes a new instance of the <see cref="ServiceFactory{TService, T1, T2}"/> class.
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

/// <summary>
/// Provides a service factory implementation for creating instances of <typeparamref name="TService"/> with three arguments.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
/// <typeparam name="T1">The first argument type.</typeparam>
/// <typeparam name="T2">The second argument type.</typeparam>
/// <typeparam name="T3">The third argument type.</typeparam>
internal sealed class ServiceFactory<TService, T1, T2, T3> : IServiceFactory<TService, T1, T2, T3>
{
    private readonly IServiceProvider provider;
    private readonly Func<IServiceProvider, T1, T2, T3, TService> factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceFactory{TService, T1, T2, T3}"/> class.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <param name="factory">The factory function to instantiate the service.</param>
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, T3, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    /// <inheritdoc />
    public TService CreateInstance(T1 arg1, T2 arg2, T3 arg3)
    {
        return this.factory(this.provider, arg1, arg2, arg3);
    }
}

/// <summary>
/// Provides a service factory implementation for creating instances of <typeparamref name="TService"/> with four arguments.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
/// <typeparam name="T1">The first argument type.</typeparam>
/// <typeparam name="T2">The second argument type.</typeparam>
/// <typeparam name="T3">The third argument type.</typeparam>
/// <typeparam name="T4">The fourth argument type.</typeparam>
internal sealed class ServiceFactory<TService, T1, T2, T3, T4> : IServiceFactory<TService, T1, T2, T3, T4>
{
    private readonly IServiceProvider provider;
    private readonly Func<IServiceProvider, T1, T2, T3, T4, TService> factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceFactory{TService, T1, T2, T3, T4}"/> class.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <param name="factory">The factory function to instantiate the service.</param>
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, T3, T4, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    /// <inheritdoc />
    public TService CreateInstance(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return this.factory(this.provider, arg1, arg2, arg3, arg4);
    }
}

/// <summary>
/// Provides a service factory implementation for creating instances of <typeparamref name="TService"/> with five arguments.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
/// <typeparam name="T1">The first argument type.</typeparam>
/// <typeparam name="T2">The second argument type.</typeparam>
/// <typeparam name="T3">The third argument type.</typeparam>
/// <typeparam name="T4">The fourth argument type.</typeparam>
/// <typeparam name="T5">The fifth argument type.</typeparam>
internal sealed class ServiceFactory<TService, T1, T2, T3, T4, T5> : IServiceFactory<TService, T1, T2, T3, T4, T5>
{
    private readonly IServiceProvider provider;
    private readonly Func<IServiceProvider, T1, T2, T3, T4, T5, TService> factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceFactory{TService, T1, T2, T3, T4, T5}"/> class.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <param name="factory">The factory function to instantiate the service.</param>
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, T3, T4, T5, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    /// <inheritdoc />
    public TService CreateInstance(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return this.factory(this.provider, arg1, arg2, arg3, arg4, arg5);
    }
}

/// <summary>
/// Provides a service factory implementation for creating instances of <typeparamref name="TService"/> with six arguments.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
/// <typeparam name="T1">The first argument type.</typeparam>
/// <typeparam name="T2">The second argument type.</typeparam>
/// <typeparam name="T3">The third argument type.</typeparam>
/// <typeparam name="T4">The third argument type.</typeparam>
/// <typeparam name="T5">The fifth argument type.</typeparam>
/// <typeparam name="T6">The sixth argument type.</typeparam>
internal sealed class ServiceFactory<TService, T1, T2, T3, T4, T5, T6> : IServiceFactory<TService, T1, T2, T3, T4, T5, T6>
{
    private readonly IServiceProvider provider;
    private readonly Func<IServiceProvider, T1, T2, T3, T4, T5, T6, TService> factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceFactory{TService, T1, T2, T3, T4, T5, T6}"/> class.
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

/// <summary>
/// Provides a service factory implementation for creating instances of <typeparamref name="TService"/> with seven arguments.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
/// <typeparam name="T1">The first argument type.</typeparam>
/// <typeparam name="T2">The second argument type.</typeparam>
/// <typeparam name="T3">The third argument type.</typeparam>
/// <typeparam name="T4">The fourth argument type.</typeparam>
/// <typeparam name="T5">The fifth argument type.</typeparam>
/// <typeparam name="T6">The sixth argument type.</typeparam>
/// <typeparam name="T7">The seventh argument type.</typeparam>
internal sealed class ServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7> : IServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7>
{
    private readonly IServiceProvider provider;
    private readonly Func<IServiceProvider, T1, T2, T3, T4, T5, T6, T7, TService> factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceFactory{TService, T1, T2, T3, T4, T5, T6, T7}"/> class.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <param name="factory">The factory function to instantiate the service.</param>
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, T3, T4, T5, T6, T7, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    /// <inheritdoc />
    public TService CreateInstance(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return this.factory(this.provider, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }
}
