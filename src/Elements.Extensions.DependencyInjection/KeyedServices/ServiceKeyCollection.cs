// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.KeyedServices;

/// <summary>
/// Provides a collection of service keys and their registered implementation types for type <typeparamref name="TService"/>.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
/// <param name="keys">The collection of key and implementation type mappings.</param>
public class ServiceKeyCollection<TService>(IReadOnlyCollection<KeyValuePair<object, Type>> keys)
    : IServiceKeyCollection<TService>
    where TService : notnull
{
    private readonly IReadOnlyCollection<KeyValuePair<object, Type>> keys = keys;

    /// <inheritdoc />
    public IEnumerator<object> GetEnumerator()
    {
        return this.keys.Select(t => t.Key).GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)this.keys).GetEnumerator();
    }

    /// <inheritdoc />
    public int Count
    {
        get
        {
            return this.keys.Count;
        }
    }

    /// <inheritdoc />
    public Type AffectedServiceType { get; } = typeof(TService);

    /// <inheritdoc />
    public bool HasType<TConcrete>() where TConcrete : TService
    {
        return this.keys.Any(t => t.Value.IsAssignableTo(typeof(TConcrete)));
    }
}
