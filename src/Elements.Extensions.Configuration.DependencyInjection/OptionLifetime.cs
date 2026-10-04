// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

namespace HedgeCraft.Elements.Extensions.Configuration.DependencyInjection;

/// <summary>
/// Defines how option values are retrieved when resolving a keyed service.
/// </summary>
public enum OptionLifetime
{
    /// <summary>
    /// Uses a single <see cref="Microsoft.Extensions.Options.IOptions{TOptions}"/> instance.
    /// </summary>
    Value,

    /// <summary>
    /// Uses a scoped snapshot of the options for the current request.
    /// </summary>
    Snapshot,

    /// <summary>
    /// Uses the current value from <see cref="Microsoft.Extensions.Options.IOptionsMonitor{TOptions}"/>.
    /// </summary>
    Monitor
}
