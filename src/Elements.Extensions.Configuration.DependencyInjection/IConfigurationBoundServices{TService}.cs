// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.Configuration.DependencyInjection;

/// <summary>
/// Configures the keyed and non-keyed registrations for a service bound to configuration values.
/// </summary>
/// <typeparam name="TService">The service type that is resolved based on selected configuration data.</typeparam>
public interface IConfigurationBoundServices<TService>
    where TService : notnull
{
    /// <summary>
    /// Registers a keyed implementation using the specified lifetime.
    /// </summary>
    /// <typeparam name="TImplementation">The concrete implementation type.</typeparam>
    /// <param name="serviceKey">The key used to resolve the keyed service.</param>
    /// <param name="serviceLifetime">The desired lifetime for the keyed registration.</param>
    /// <returns>The current services builder for additional registrations.</returns>
    IConfigurationBoundServices<TService> AddKeyed<TImplementation>(object serviceKey, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TImplementation : class, TService;

    /// <summary>
    /// Registers a keyed implementation as a scoped service.
    /// </summary>
    /// <typeparam name="TImplementation">The concrete implementation type.</typeparam>
    /// <param name="serviceKey">The key used to resolve the keyed service.</param>
    /// <returns>The current services builder for additional registrations.</returns>
    IConfigurationBoundServices<TService> AddKeyedScoped<TImplementation>(object serviceKey)
        where TImplementation : class, TService;

    /// <summary>
    /// Registers a keyed implementation as a singleton service.
    /// </summary>
    /// <typeparam name="TImplementation">The concrete implementation type.</typeparam>
    /// <param name="serviceKey">The key used to resolve the keyed service.</param>
    /// <returns>The current services builder for additional registrations.</returns>
    IConfigurationBoundServices<TService> AddKeyedSingleton<TImplementation>(object serviceKey)
        where TImplementation : class, TService;

    /// <summary>
    /// Registers a keyed implementation as a transient service.
    /// </summary>
    /// <typeparam name="TImplementation">The concrete implementation type.</typeparam>
    /// <param name="serviceKey">The key used to resolve the keyed service.</param>
    /// <returns>The current services builder for additional registrations.</returns>
    IConfigurationBoundServices<TService> AddKeyedTransient<TImplementation>(object serviceKey)
        where TImplementation : class, TService;

    /// <summary>
    /// Adds the main service registration using the configured option lifetime.
    /// </summary>
    /// <param name="serviceLifetime">The requested lifetime for the primary registration.</param>
    /// <returns>The service collection used to register the component.</returns>
    IServiceCollection AddServiceWithLifetime(ServiceLifetime serviceLifetime);

    /// <summary>
    /// Adds the main service registration as a scoped service.
    /// </summary>
    /// <returns>The service collection used to register the component.</returns>
    IServiceCollection AddScopedService();

    /// <summary>
    /// Adds the main service registration as a singleton service.
    /// </summary>
    /// <returns>The service collection used to register the component.</returns>
    IServiceCollection AddSingletonService();

    /// <summary>
    /// Adds the main service registration as a transient service.
    /// </summary>
    /// <returns>The service collection used to register the component.</returns>
    IServiceCollection AddTransientService();
}
