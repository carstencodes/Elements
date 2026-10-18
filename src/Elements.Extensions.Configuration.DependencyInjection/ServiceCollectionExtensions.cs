// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HedgeCraft.Elements.Extensions.Configuration.DependencyInjection;

/// <summary>
/// Extension methods for registering configuration-bound services in the dependency injection container.
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a service whose implementation is selected from a keyed registration based on the bound options object.
    /// </summary>
    /// <typeparam name="TService">The service type being registered.</typeparam>
    /// <typeparam name="TOptions">The options type bound from configuration.</typeparam>
    /// <param name="services">The service collection to which registrations are added.</param>
    /// <param name="getServiceKey">A callback that derives the keyed service identifier from the options.</param>
    /// <param name="configurationSection">The optional configuration section used to bind the options.</param>
    /// <param name="configureOptions">An optional options builder callback for additional configuration.</param>
    /// <returns>A builder for configuring the resulting keyed and non-keyed registrations.</returns>
    public static IConfigurationBoundServicesBuilder<TService> AddServiceFromOptions<TService, TOptions>(
        this IServiceCollection services,
        Func<TOptions, object?> getServiceKey,
        IConfigurationSection? configurationSection = null,
        Action<OptionsBuilder<TOptions>>? configureOptions = null)
        where TService : class
        where TOptions : class, new()
    {
        OptionsBuilder<TOptions> builder = services
            .AddOptions<TOptions>();
        if (configurationSection is not null)
        {
            builder = builder.Bind(configurationSection);
        }

        configureOptions?.Invoke(builder);

        return new ConfigurationBoundServicesBuilder<TService, TOptions>(services, getServiceKey);
    }
}
