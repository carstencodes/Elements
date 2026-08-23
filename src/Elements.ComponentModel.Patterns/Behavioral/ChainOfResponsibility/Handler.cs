// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

/// <summary>
/// Coordinates the execution of requests through a chain of responsibility without a return value.
/// </summary>
public sealed class Handler : HandlerBase
{
    private readonly HandlerChainLink handlerImplementation;

    /// <summary>
    /// Initializes a new instance of the <see cref="Handler"/> class with the initial chain link.
    /// </summary>
    /// <param name="handlerImplementation">The starting link of the handler chain.</param>
    public Handler(HandlerChainLink handlerImplementation)
    {
        this.handlerImplementation = handlerImplementation;
    }

    /// <summary>
    /// Processes the request across the chain of responsibility.
    /// </summary>
    public void HandleRequest()
    {
        try
        {
            this.handlerImplementation.HandleCurrentRequest();
        }
        catch (Exception e) when (HandlerBase.ShouldStop(e))
        {
            // Intentionally left blank - handler should stop
        }
    }
}
