// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

namespace HedgeCraft.Elements.Extensions.Configuration.DependencyInjection;

/// <summary>
/// Builds a configuration-aware service registration pipeline for a service type.
/// </summary>
/// <typeparam name="TService">The target service type to register.</typeparam>
public interface IConfigurationBoundServicesBuilder<TService>
    where TService : notnull
{
    /// <summary>
    /// Sets the lifetime of the configuration options used when resolving the service.
    /// </summary>
    /// <param name="optionLifetime">The option lifetime to use for the service factory.</param>
    /// <returns>The current builder instance.</returns>
    IConfigurationBoundServicesBuilder<TService> WithOptionLifetime(OptionLifetime optionLifetime);

    /// <summary>
    /// Creates the configuration-bound service registration object.
    /// </summary>
    /// <returns>The configured registration builder.</returns>
    IConfigurationBoundServices<TService> Build();
}
