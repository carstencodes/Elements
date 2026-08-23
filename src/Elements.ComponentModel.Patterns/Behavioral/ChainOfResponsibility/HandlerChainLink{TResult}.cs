// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

/// <summary>
/// Represents a link in a chain of responsibility for request handling that produces a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the result produced by handling the request.</typeparam>
public readonly struct HandlerChainLink<TResult> : IHandler<HandlerChainLink<TResult>, TResult>, IEquatable<HandlerChainLink<TResult>> where TResult : notnull
{
    private readonly Func<TResult> handleRequest;
    private readonly Func<bool> canHandleRequest;

    /// <summary>
    /// Represents an empty or terminal handler link.
    /// </summary>
    public static readonly HandlerChainLink<TResult> None = new(Nothing, Never);

    /// <summary>
    /// Initializes a new instance of the <see cref="HandlerChainLink{TResult}"/> struct with handler delegates and a successor link.
    /// </summary>
    /// <param name="handleRequest">The function to produce a result when handling the request.</param>
    /// <param name="canHandleRequest">The predicate to determine if this link can handle the request.</param>
    /// <param name="next">The next handler link in the chain.</param>
    public HandlerChainLink(Func<TResult> handleRequest, Func<bool> canHandleRequest, HandlerChainLink<TResult> next) : this(handleRequest, canHandleRequest)
    {
        this.Next = next;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HandlerChainLink{TResult}"/> struct with handler delegates.
    /// </summary>
    /// <param name="handleRequest">The function to produce a result when handling the request.</param>
    /// <param name="canHandleRequest">The predicate to determine if this link can handle the request.</param>
    public HandlerChainLink(Func<TResult> handleRequest, Func<bool> canHandleRequest)
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
    /// Handles the current request and produces a result, or forwards it to the successor link in the chain.
    /// </summary>
    /// <returns>The result produced by handling the request.</returns>
    /// <exception cref="HandlerBase.HandlerReachedEndOfChainException">Thrown when no link in the chain can handle the request.</exception>
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

    /// <summary>
    /// Gets the next handler link in the chain.
    /// </summary>
    public IHandler<HandlerChainLink<TResult>, TResult> Next { get; } = None;

    /// <summary>
    /// Indicates whether the current link is equal to another link.
    /// </summary>
    /// <param name="other">The link to compare with.</param>
    /// <returns><see langword="true"/> if equal; otherwise, <see langword="false"/>.</returns>
    bool IEquatable<HandlerChainLink<TResult>>.Equals(HandlerChainLink<TResult> other)
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

    private static TResult Nothing()
    {
        throw new NotSupportedException("This should never happen");
    }

    /// <summary>
    /// Compares two <see cref="HandlerChainLink{TResult}"/> instances for equality.
    /// </summary>
    /// <param name="left">The first link to compare.</param>
    /// <param name="right">The second link to compare.</param>
    /// <returns><see langword="true"/> if equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(HandlerChainLink<TResult> left, HandlerChainLink<TResult> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two <see cref="HandlerChainLink{TResult}"/> instances for inequality.
    /// </summary>
    /// <param name="left">The first link to compare.</param>
    /// <param name="right">The second link to compare.</param>
    /// <returns><see langword="true"/> if not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(HandlerChainLink<TResult> left, HandlerChainLink<TResult> right)
    {
        return !(left == right);
    }
}
