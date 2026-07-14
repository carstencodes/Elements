// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

public sealed class Handler : HandlerBase
{
    private readonly HandlerChainLink handlerImplementation;

    public Handler(HandlerChainLink handlerImplementation)
    {
        this.handlerImplementation = handlerImplementation;
    }

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
