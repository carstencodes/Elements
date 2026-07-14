// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

public readonly struct HandlerChainLink : IHandler<HandlerChainLink>, IEquatable<HandlerChainLink>
{
    private readonly Action handleRequest;
    private readonly Func<bool> canHandleRequest;

    public static readonly HandlerChainLink None = new(Nothing, Never);

    public HandlerChainLink(Action handleRequest, Func<bool> canHandleRequest, HandlerChainLink next) : this(handleRequest, canHandleRequest)
    {
        this.Next = next;
    }

    public HandlerChainLink(Action handleRequest, Func<bool> canHandleRequest)
    {
        this.handleRequest = handleRequest;
        this.canHandleRequest = canHandleRequest;
    }

    public bool CanHandleCurrentRequest()
    {
        return this.canHandleRequest();
    }

    public void HandleCurrentRequest()
    {
        if (this.CanHandleCurrentRequest())
        {
            this.HandleCurrentRequest();
            return;
        }

        if (!HandlerChainLink.None.Equals(this.Next))
        {
            this.Next.HandleCurrentRequest();
            return;
        }

        HandlerBase.StopHandling();
    }

    public IHandler<HandlerChainLink> Next { get; } = None;

    bool IEquatable<HandlerChainLink>.Equals(HandlerChainLink other)
    {
        return this.handleRequest.Equals(other.handleRequest) && this.canHandleRequest.Equals(other.canHandleRequest) && this.Next.Equals(other.Next);
    }

    public override bool Equals(object? obj)
    {
        return obj is HandlerChainLink other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.handleRequest, this.canHandleRequest, this.Next);
    }

    private static bool Never()
    {
        return false;
    }

    private static void Nothing()
    {
        throw new NotSupportedException("This should never happen");
    }

    public static bool operator ==(HandlerChainLink left, HandlerChainLink right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(HandlerChainLink left, HandlerChainLink right)
    {
        return !(left == right);
    }
}
