using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace Elements.Extensions.DependencyInjection.KeyedServices;

public sealed class KeyedServiceCollection<TService>: IKeyedServiceCollection<TService> where TService: notnull
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
        return GetEnumerator();
    }

    public int Count => this.keys.Count;

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