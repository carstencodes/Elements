using System;

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

public readonly struct HandlerChainLink<TResult>: IHandler<HandlerChainLink<TResult>, TResult>, IEquatable<HandlerChainLink<TResult>> where TResult: notnull
{
    private readonly Func<TResult> handleRequest;
    private readonly Func<bool> canHandleRequest;
    
    public static readonly HandlerChainLink<TResult> None = new(Nothing, Never);
    
    public HandlerChainLink(Func<TResult> handleRequest, Func<bool> canHandleRequest, HandlerChainLink<TResult> next): this(handleRequest, canHandleRequest)
    {
        this.Next = next;
    }
    
    public HandlerChainLink(Func<TResult> handleRequest, Func<bool> canHandleRequest)
    {
        this.handleRequest = handleRequest;
        this.canHandleRequest = canHandleRequest;
    }
    
    public bool CanHandleCurrentRequest()
    {
        return this.canHandleRequest();
    }

    public TResult HandleCurrentRequest()
    {
        if (this.CanHandleCurrentRequest())
        {
            return this.HandleCurrentRequest();
        }

        if (!HandlerChainLink<TResult>.None.Equals(this.Next))
        {
            return this.Next.HandleCurrentRequest();
        }

        HandlerBase.StopHandling();
        throw new NotSupportedException(); // heuristically unreachable
    }

    public IHandler<HandlerChainLink<TResult>, TResult> Next { get; } = None;

    bool IEquatable<HandlerChainLink<TResult>>.Equals(HandlerChainLink<TResult> other)
    {
        return handleRequest.Equals(other.handleRequest) && canHandleRequest.Equals(other.canHandleRequest) && Next.Equals(other.Next);
    }

    public override bool Equals(object? obj)
    {
        return obj is HandlerChainLink other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(handleRequest, canHandleRequest, Next);
    }

    private static bool Never()
    {
        return false;
    }

    private static TResult Nothing()
    {
        throw new NotSupportedException("This should never happen");
    }
}