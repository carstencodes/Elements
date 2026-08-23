// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

/// <summary>
/// Represents a link in a chain of responsibility for request handling without a return value.
/// </summary>
public readonly struct HandlerChainLink : IHandler<HandlerChainLink>, IEquatable<HandlerChainLink>
{
    private readonly Action handleRequest;
    private readonly Func<bool> canHandleRequest;

    /// <summary>
    /// Represents an empty or terminal handler link.
    /// </summary>
    public static readonly HandlerChainLink None = new(Nothing, Never);

    /// <summary>
    /// Initializes a new instance of the <see cref="HandlerChainLink"/> struct with handler delegates and a successor link.
    /// </summary>
    /// <param name="handleRequest">The action to execute when handling the request.</param>
    /// <param name="canHandleRequest">The predicate to determine if this link can handle the request.</param>
    /// <param name="next">The next handler link in the chain.</param>
    public HandlerChainLink(Action handleRequest, Func<bool> canHandleRequest, HandlerChainLink next) : this(handleRequest, canHandleRequest)
    {
        this.Next = next;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HandlerChainLink"/> struct with handler delegates.
    /// </summary>
    /// <param name="handleRequest">The action to execute when handling the request.</param>
    /// <param name="canHandleRequest">The predicate to determine if this link can handle the request.</param>
    public HandlerChainLink(Action handleRequest, Func<bool> canHandleRequest)
    {
        this.handleRequest = handleRequest;
        this.canHandleRequest = canHandleRequest;
    }

    /// <summary>
    /// Determines whether this link can handle the current request.
    /// </summary>
    /// <returns><see langword="true"/> if the link can handle the request; otherwise, <see langword="false"/>.</returns>
    public bool CanHandleCurrentRequest()
    {
        return this.canHandleRequest();
    }

    /// <summary>
    /// Handles the current request or forwards it to the successor link in the chain.
    /// </summary>
    /// <exception cref="HandlerBase.HandlerReachedEndOfChainException">Thrown when no link in the chain can handle the request.</exception>
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

    /// <summary>
    /// Gets the next handler link in the chain.
    /// </summary>
    public IHandler<HandlerChainLink> Next { get; } = None;

    /// <summary>
    /// Indicates whether the current link is equal to another link.
    /// </summary>
    /// <param name="other">The link to compare with.</param>
    /// <returns><see langword="true"/> if equal; otherwise, <see langword="false"/>.</returns>
    bool IEquatable<HandlerChainLink>.Equals(HandlerChainLink other)
    {
        return this.handleRequest.Equals(other.handleRequest) && this.canHandleRequest.Equals(other.canHandleRequest) && this.Next.Equals(other.Next);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current link.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns><see langword="true"/> if equal; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is HandlerChainLink other && this.Equals(other);
    }

    /// <summary>
    /// Returns the hash code for this link.
    /// </summary>
    /// <returns>A hash code integer.</returns>
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

    /// <summary>
    /// Compares two <see cref="HandlerChainLink"/> instances for equality.
    /// </summary>
    /// <param name="left">The first link to compare.</param>
    /// <param name="right">The second link to compare.</param>
    /// <returns><see langword="true"/> if equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(HandlerChainLink left, HandlerChainLink right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two <see cref="HandlerChainLink"/> instances for inequality.
    /// </summary>
    /// <param name="left">The first link to compare.</param>
    /// <param name="right">The second link to compare.</param>
    /// <returns><see langword="true"/> if not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(HandlerChainLink left, HandlerChainLink right)
    {
        return !(left == right);
    }
}
