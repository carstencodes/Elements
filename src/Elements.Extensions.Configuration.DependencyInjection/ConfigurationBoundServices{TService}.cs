// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.Configuration.DependencyInjection;

/// <summary>
/// Represents the configuration-bound registrations for a service type.
/// </summary>
/// <typeparam name="TService">The concrete service type managed by the registrations.</typeparam>
internal sealed class ConfigurationBoundServices<TService>(Func<IServiceProvider, TService> createService, Action<IServiceCollection, OptionLifetime, ServiceLifetime> selectFactoryFromOptionLifetime, OptionLifetime optionLifetime, IServiceCollection services): IConfigurationBoundServices<TService>
    where TService : class
{
    /// <summary>
    /// Registers a keyed implementation for the specified lifetime.
    /// </summary>
    /// <typeparam name="TImplementation">The concrete implementation type to register.</typeparam>
    /// <param name="serviceKey">The key used to resolve the keyed dependency.</param>
    /// <param name="serviceLifetime">The lifetime to apply to the keyed registration.</param>
    /// <returns>The current registration builder.</returns>
    public IConfigurationBoundServices<TService> AddKeyed<TImplementation>(object serviceKey, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TImplementation : class, TService
    {
        return serviceLifetime switch 
        {
            ServiceLifetime.Scoped => this.AddKeyedScoped<TImplementation>(serviceKey),
            ServiceLifetime.Singleton => this.AddKeyedSingleton<TImplementation>(serviceKey),
            ServiceLifetime.Transient => this.AddKeyedTransient<TImplementation>(serviceKey),
            _ => throw new ArgumentException($"Unsupported service lifetime: {serviceLifetime}", nameof(serviceLifetime))
        };
    }

    /// <summary>
    /// Registers a keyed implementation as a scoped service.
    /// </summary>
    /// <typeparam name="TImplementation">The concrete implementation type to register.</typeparam>
    /// <param name="serviceKey">The key used to resolve the keyed dependency.</param>
    /// <returns>The current registration builder.</returns>
    public IConfigurationBoundServices<TService> AddKeyedScoped<TImplementation>(object serviceKey)
        where TImplementation : class, TService
    {
        services.AddKeyedScoped<TService, TImplementation>(serviceKey);
        return this;
    }

    /// <summary>
    /// Registers a keyed implementation as a singleton service.
    /// </summary>
    /// <typeparam name="TImplementation">The concrete implementation type to register.</typeparam>
    /// <param name="serviceKey">The key used to resolve the keyed dependency.</param>
    /// <returns>The current registration builder.</returns>
    public IConfigurationBoundServices<TService> AddKeyedSingleton<TImplementation>(object serviceKey)
        where TImplementation : class, TService
    {
        services.AddKeyedSingleton<TService, TImplementation>(serviceKey);
        return this;
    }

    /// <summary>
    /// Registers a keyed implementation as a transient service.
    /// </summary>
    /// <typeparam name="TImplementation">The concrete implementation type to register.</typeparam>
    /// <param name="serviceKey">The key used to resolve the keyed dependency.</param>
    /// <returns>The current registration builder.</returns>
    public IConfigurationBoundServices<TService> AddKeyedTransient<TImplementation>(object serviceKey)
        where TImplementation : class, TService
    {
        services.AddKeyedTransient<TService, TImplementation>(serviceKey);
        return this;
    }

    /// <summary>
    /// Adds the main service registration using the configured option lifetime.
    /// </summary>
    /// <param name="serviceLifetime">The requested lifetime for the service registration.</param>
    /// <returns>The updated service collection.</returns>
    public IServiceCollection AddServiceWithLifetime(ServiceLifetime serviceLifetime) 
    {
        selectFactoryFromOptionLifetime(services, optionLifetime, serviceLifetime);

        return serviceLifetime switch 
        {
            ServiceLifetime.Scoped => services.AddScoped<TService>(createService),
            ServiceLifetime.Singleton => services.AddSingleton<TService>(createService),
            ServiceLifetime.Transient => services.AddTransient<TService>(createService),
            _ => services,
        };
    }

    /// <summary>
    /// Adds the main service registration as a scoped service.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    public IServiceCollection AddScopedService() 
    {
        return this.AddServiceWithLifetime(ServiceLifetime.Scoped);
    }

    /// <summary>
    /// Adds the main service registration as a singleton service.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    public IServiceCollection AddSingletonService() 
    {
        return this.AddServiceWithLifetime(ServiceLifetime.Singleton);
    }

    /// <summary>
    /// Adds the main service registration as a transient service.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    public IServiceCollection AddTransientService() 
    {
        return this.AddServiceWithLifetime(ServiceLifetime.Transient);
    }
}
