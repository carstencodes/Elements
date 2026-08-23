// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.KeyedServices;

/// <summary>
/// Provides a collection of keyed services resolved via an <see cref="IKeyedServiceProvider"/>.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
public sealed class KeyedServiceCollection<TService> : IKeyedServiceCollection<TService> where TService : notnull
{
    private readonly IKeyedServiceProvider provider;
    private readonly IServiceKeyCollection<TService> keys;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyedServiceCollection{TService}"/> class.
    /// </summary>
    /// <param name="provider">The keyed service provider.</param>
    /// <param name="keys">The collection of keys associated with the service.</param>
    public KeyedServiceCollection(IKeyedServiceProvider provider, IServiceKeyCollection<TService> keys)
    {
        this.provider = provider;
        this.keys = keys;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the registered keyed service instances.
    /// </summary>
    /// <returns>An enumerator for the service instances.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when a required keyed service cannot be resolved.</exception>
    public IEnumerator<TService> GetEnumerator()
    {
        return this.keys.Select(this.provider.GetRequiredKeyedService<TService>).GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Gets the number of registered keys for this service.
    /// </summary>
    public int Count
    {
        get
        {
            return this.keys.Count;
        }
    }

    /// <inheritdoc />
    public IReadOnlyCollection<object> GetAllKeys()
    {
        return this.keys;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<TKey> GetAllKeysOfType<TKey>() where TKey : notnull
    {
        return this.keys.OfType<TKey>().ToArray();
    }

    /// <inheritdoc />
    public bool HasKey(object key)
    {
        return this.keys.Contains(key);
    }

    /// <inheritdoc />
    public TService? GetService(object key)
    {
        return this.provider.GetKeyedService<TService>(key);
    }

    /// <inheritdoc />
    public TService GetRequiredService(object key)
    {
        return this.provider.GetRequiredKeyedService<TService>(key);
    }
}
