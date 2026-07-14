// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections.Generic;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.KeyedServices;

public interface IKeyedServiceCollection<out TService> : IReadOnlyCollection<TService> where TService : notnull
{
    IReadOnlyCollection<object> GetAllKeys();

    IReadOnlyCollection<TKey> GetAllKeysOfType<TKey>() where TKey : notnull;
    bool HasKey(object key);

    TService? GetService(object key);

    TService GetRequiredService(object key);
}
