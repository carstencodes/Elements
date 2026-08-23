// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

/// <summary>
/// Coordinates the execution of requests through a chain of responsibility, returning a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the result produced by handling the request.</typeparam>
public sealed class ResultedHandler<TResult> : HandlerBase where TResult : notnull
{
    private readonly HandlerChainLink<TResult> handlerImplementation;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultedHandler{TResult}"/> class with the initial chain link.
    /// </summary>
    /// <param name="handlerImplementation">The starting link of the handler chain.</param>
    public ResultedHandler(HandlerChainLink<TResult> handlerImplementation)
    {
        this.handlerImplementation = handlerImplementation;
    }

    /// <summary>
    /// Processes the request across the chain of responsibility and returns the result.
    /// </summary>
    /// <returns>The result produced by the handling chain, or <see langword="default"/> if no handler handled the request.</returns>
    public TResult? HandleRequest()
    {
        try
        {
            return this.handlerImplementation.HandleCurrentRequest();
        }
        catch (Exception e) when (HandlerBase.ShouldStop(e))
        {
            return default;
        }
    }

    /// <summary>
    /// Processes the request across the chain of responsibility, returning a fallback value if unhandled.
    /// </summary>
    /// <param name="defaultValue">The fallback value to return when no handler processes the request.</param>
    /// <returns>The result produced by the handling chain, or <paramref name="defaultValue"/> if unhandled.</returns>
    public TResult HandleRequestOrDefault(TResult defaultValue)
    {
        return this.HandleRequest() ?? defaultValue;
    }
}
