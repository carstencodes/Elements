// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections.Generic;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.KeyedServices;

/// <summary>
/// Defines a collection of keyed services allowing lookup by service key.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
public interface IKeyedServiceCollection<out TService> : IReadOnlyCollection<TService> where TService : notnull
{
    /// <summary>
    /// Gets all registered keys for this service type.
    /// </summary>
    /// <returns>A collection of all service keys.</returns>
    IReadOnlyCollection<object> GetAllKeys();

    /// <summary>
    /// Gets all registered keys of a specific type for this service type.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys to retrieve.</typeparam>
    /// <returns>A collection of keys of type <typeparamref name="TKey"/>.</returns>
    IReadOnlyCollection<TKey> GetAllKeysOfType<TKey>() where TKey : notnull;

    /// <summary>
    /// Determines whether a service is registered with the specified key.
    /// </summary>
    /// <param name="key">The key to search for.</param>
    /// <returns><see langword="true"/> if a service is registered with the specified key; otherwise, <see langword="false"/>.</returns>
    bool HasKey(object key);

    /// <summary>
    /// Retrieves the service registered with the specified key, or <see langword="null"/> if not registered.
    /// </summary>
    /// <param name="key">The key of the service.</param>
    /// <returns>The registered service instance, or <see langword="null"/> if not found.</returns>
    TService? GetService(object key);

    /// <summary>
    /// Retrieves the service registered with the specified key.
    /// </summary>
    /// <param name="key">The key of the service.</param>
    /// <returns>The registered service instance.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when no service is registered for the specified key.</exception>
    TService GetRequiredService(object key);
}
