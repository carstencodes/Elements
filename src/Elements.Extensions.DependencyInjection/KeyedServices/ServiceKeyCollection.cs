using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.KeyedServices;

public class ServiceKeyCollection<TService>(IReadOnlyCollection<KeyValuePair<object, Type>> keys)
    : IServiceKeyCollection<TService>
    where TService : notnull
{
    private readonly IReadOnlyCollection<KeyValuePair<object, Type>> keys = keys;

    public IEnumerator<object> GetEnumerator()
    {
        return this.keys.Select(t => t.Key).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)this.keys).GetEnumerator();
    }

    public int Count
    {
        get
        {
            return this.keys.Count;
        }
    }

    public Type AffectedServiceType { get; } = typeof(TService);
    public bool HasType<TConcrete>() where TConcrete : TService
    {
        return this.keys.Any(t => t.Value.IsAssignableTo(typeof(TConcrete)));
    }
}
