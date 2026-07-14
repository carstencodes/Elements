// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.KeyedServices;

public sealed class KeyedServiceCollection<TService> : IKeyedServiceCollection<TService> where TService : notnull
{
    private readonly IKeyedServiceProvider provider;
    private readonly IServiceKeyCollection<TService> keys;

    public KeyedServiceCollection(IKeyedServiceProvider provider, IServiceKeyCollection<TService> keys)
    {
        this.provider = provider;
        this.keys = keys;
    }

    public IEnumerator<TService> GetEnumerator()
    {
        return this.keys.Select(this.provider.GetRequiredKeyedService<TService>).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public int Count
    {
        get
        {
            return this.keys.Count;
        }
    }

    public IReadOnlyCollection<object> GetAllKeys()
    {
        return this.keys;
    }

    public IReadOnlyCollection<TKey> GetAllKeysOfType<TKey>() where TKey : notnull
    {
        return this.keys.OfType<TKey>().ToArray();
    }

    public bool HasKey(object key)
    {
        return this.keys.Contains(key);
    }

    public TService? GetService(object key)
    {
        return this.provider.GetKeyedService<TService>(key);
    }

    public TService GetRequiredService(object key)
    {
        return this.provider.GetRequiredKeyedService<TService>(key);
    }
}
