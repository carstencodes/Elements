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
    IHandler<TSelf>? Next { get; }
}

/// <summary>
/// Defines a handler link in a chain of responsibility that produces a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TSelf">The concrete handler type.</typeparam>
/// <typeparam name="TResult">The type of the result produced by handling the request.</typeparam>
public interface IHandler<out TSelf, out TResult> where TSelf : IHandler<TSelf, TResult> where TResult : notnull
{
    /// <summary>
    /// Determines whether the handler can process the current request.
    /// </summary>
    /// <returns><see langword="true"/> if the handler can process the request; otherwise, <see langword="false"/>.</returns>
    bool CanHandleCurrentRequest();

    /// <summary>
    /// Processes the current request and returns a result.
    /// </summary>
    /// <returns>The result produced by handling the request.</returns>
    TResult HandleCurrentRequest();

    /// <summary>
    /// Gets the next handler in the chain.
    /// </summary>
    IHandler<TSelf, TResult>? Next { get; }
}
