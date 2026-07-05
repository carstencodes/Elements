using System;

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

public sealed class ResultedHandler<TResult>: HandlerBase where TResult: notnull
{
    private readonly HandlerChainLink<TResult> handlerImplementation;

    public ResultedHandler(HandlerChainLink<TResult> handlerImplementation)
    {
        this.handlerImplementation = handlerImplementation;
    }
    
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

    public TResult HandleRequestOrDefault(TResult defaultValue)
    {
        return this.HandleRequest() ?? defaultValue;
    }
}