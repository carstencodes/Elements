// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HedgeCraft.Elements.Extensions.Configuration.DependencyInjection;

/// <summary>
/// Resolves a keyed service based on the current options value selected for the configured lifetime.
/// </summary>
/// <typeparam name="TService">The service type to resolve.</typeparam>
/// <typeparam name="TOptions">The configuration options type used to derive the service key.</typeparam>
internal sealed class ConfigurableServiceFactory<TService, TOptions>(Func<TOptions> getOptions)
    where TService : class
    where TOptions : class, new()
{
    /// <summary>
    /// Initializes the factory from a single options instance.
    /// </summary>
    /// <param name="options">The options instance to read.</param>
    private ConfigurableServiceFactory(IOptions<TOptions> options) : this(() => options.Value)
    {
    }

    /// <summary>
    /// Initializes the factory from a scoped options snapshot.
    /// </summary>
    /// <param name="options">The options snapshot to read.</param>
    private ConfigurableServiceFactory(IOptionsSnapshot<TOptions> options) : this(() => options.Value)
    {
    }

    /// <summary>
    /// Initializes the factory from the current monitor value.
    /// </summary>
    /// <param name="options">The options monitor to read.</param>
    private ConfigurableServiceFactory(IOptionsMonitor<TOptions> options) : this(() => options.CurrentValue)
    {
    }

    /// <summary>
    /// Registers the factory implementation for the selected option lifetime and service lifetime.
    /// </summary>
    /// <param name="services">The service collection receiving the configuration-bound factory.</param>
    /// <param name="optionLifetime">The selected option lifetime to use when resolving values.</param>
    /// <param name="serviceLifetime">The lifetime for the factory registration.</param>
    internal static void ChooseFactory(IServiceCollection services, OptionLifetime optionLifetime, ServiceLifetime serviceLifetime)
    {
        Func<IServiceProvider, Func<TOptions>> getOptions = optionLifetime switch
        {
            OptionLifetime.Value => provider =>
            {
                IOptions<TOptions> options = provider.GetRequiredService<IOptions<TOptions>>();
                return () => options.Value;
            }
            ,
            OptionLifetime.Snapshot => provider =>
            {
                IOptionsSnapshot<TOptions> options = provider.GetRequiredService<IOptionsSnapshot<TOptions>>();
                return () => options.Value;
            }
            ,
            OptionLifetime.Monitor => provider =>
            {
                IOptionsMonitor<TOptions> options = provider.GetRequiredService<IOptionsMonitor<TOptions>>();
                return () => options.CurrentValue;
            }
            ,
            _ => throw new ArgumentException($"Invalid Option lifetime: {optionLifetime}", nameof(optionLifetime)),
        };

        switch (serviceLifetime)
        {
            case ServiceLifetime.Scoped:
                services.AddScoped(provider => new ConfigurableServiceFactory<TService, TOptions>(getOptions(provider)));
                break;
            case ServiceLifetime.Singleton:
                services.AddSingleton(provider => new ConfigurableServiceFactory<TService, TOptions>(getOptions(provider)));
                break;
            case ServiceLifetime.Transient:
                services.AddTransient(provider => new ConfigurableServiceFactory<TService, TOptions>(getOptions(provider)));
                break;
            default:
                throw new ArgumentException($"Invalid Service lifetime: {serviceLifetime}", nameof(serviceLifetime));
        }
    }

    /// <summary>
    /// Resolves the keyed service from the current options instance.
    /// </summary>
    /// <param name="serviceProvider">The current dependency injection provider.</param>
    /// <param name="getServiceKey">A callback mapping the options to a keyed service identifier.</param>
    /// <returns>The keyed service instance associated with the current options.</returns>
    internal TService CreateFromServiceProvider(IServiceProvider serviceProvider, Func<TOptions, object?> getServiceKey)
    {
        TOptions options = getOptions();
        object? serviceKey = getServiceKey(options);
        return serviceProvider.GetRequiredKeyedService<TService>(serviceKey);
    }
}
