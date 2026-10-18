// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.Configuration.DependencyInjection;

/// <summary>
/// Builds configuration-bound service registrations using a selected option lifetime.
/// </summary>
/// <typeparam name="TService">The service type being registered.</typeparam>
/// <typeparam name="TOptions">The configuration options type bound from the application configuration.</typeparam>
internal sealed class ConfigurationBoundServicesBuilder<TService, TOptions>(
        IServiceCollection services,
        Func<TOptions, object?> getServiceKey,
        OptionLifetime selectedLifetime = OptionLifetime.Value)
        : IConfigurationBoundServicesBuilder<TService>
    where TService : class
    where TOptions : class, new()
{
    /// <summary>
    /// Stores the lifetime used to resolve configuration values for the managed service.
    /// </summary>
    private OptionLifetime optionLifetime = selectedLifetime;

    /// <summary>
    /// Sets the lifetime for the configuration options used during resolution.
    /// </summary>
    /// <param name="optionLifetime">The option lifetime to apply.</param>
    /// <returns>The current builder instance.</returns>
    public IConfigurationBoundServicesBuilder<TService> WithOptionLifetime(OptionLifetime optionLifetime)
    {
        this.optionLifetime = optionLifetime;
        return this;
    }

    /// <summary>
    /// Builds the configuration-bound service registration object.
    /// </summary>
    /// <returns>The configured service registration builder.</returns>
    public IConfigurationBoundServices<TService> Build()
    {
        return new ConfigurationBoundServices<TService>(this.CreateServiceFromFactory, ConfigurableServiceFactory<TService, TOptions>.ChooseFactory, this.optionLifetime, services);
    }

    /// <summary>
    /// Creates the service instance by resolving the options-backed factory from the provider.
    /// </summary>
    /// <param name="serviceProvider">The current dependency injection provider.</param>
    /// <returns>The configured service instance.</returns>
    private TService CreateServiceFromFactory(IServiceProvider serviceProvider)
    {
        ConfigurableServiceFactory<TService, TOptions> factory = serviceProvider.GetRequiredService<ConfigurableServiceFactory<TService, TOptions>>();
        return factory.CreateFromServiceProvider(serviceProvider, getServiceKey);
    }
}
