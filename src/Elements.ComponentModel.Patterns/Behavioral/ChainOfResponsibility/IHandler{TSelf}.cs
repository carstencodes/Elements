// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

/// <summary>
/// Defines a handler link in a chain of responsibility without a return value.
/// </summary>
/// <typeparam name="TSelf">The concrete handler type.</typeparam>
public interface IHandler<out TSelf> where TSelf : IHandler<TSelf>
{
    /// <summary>
    /// Determines whether the handler can process the current request.
    /// </summary>
    /// <returns><see langword="true"/> if the handler can process the request; otherwise, <see langword="false"/>.</returns>
    bool CanHandleCurrentRequest();

    /// <summary>
    /// Processes the current request.
    /// </summary>
    void HandleCurrentRequest();

    /// <summary>
    /// Gets the next handler in the chain.
    /// </summary>
    IHandler<TSelf>? NextHandler { get; }
}
