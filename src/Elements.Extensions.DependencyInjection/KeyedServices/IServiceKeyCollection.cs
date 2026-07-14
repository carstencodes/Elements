// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.KeyedServices;

public interface IServiceKeyCollection<in TService> : IReadOnlyCollection<object> where TService : notnull
{
    Type AffectedServiceType { get; }

    bool HasType<TConcrete>() where TConcrete : TService;
}
