// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.KeyedServices;

/// <summary>
/// Defines a collection of keys associated with service registrations for type <typeparamref name="TService"/>.
/// </summary>
/// <typeparam name="TService">The service type.</typeparam>
public interface IServiceKeyCollection<in TService> : IReadOnlyCollection<object> where TService : notnull
{
    /// <summary>
    /// Gets the service type associated with these keys.
    /// </summary>
    Type AffectedServiceType { get; }

    /// <summary>
    /// Determines whether the key collection contains a registration for the specified concrete implementation type.
    /// </summary>
    /// <typeparam name="TConcrete">The concrete implementation type to check.</typeparam>
    /// <returns><see langword="true"/> if registered; otherwise, <see langword="false"/>.</returns>
    bool HasType<TConcrete>() where TConcrete : TService;
}
