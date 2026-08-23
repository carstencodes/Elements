// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Diagnostics.CodeAnalysis;

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

/// <summary>
/// Provides common base functionality and exception handling mechanisms for chain of responsibility handlers.
/// </summary>
public abstract class HandlerBase
{
    /// <summary>
    /// Represents the exception thrown when a request reaches the end of the chain of responsibility without being handled.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public sealed class HandlerReachedEndOfChainException(Exception innerException)
        : Exception("The handler has no other handler in its chain", innerException)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerReachedEndOfChainException"/> class.
        /// </summary>
        public HandlerReachedEndOfChainException() : this(null!)
        {
        }
    }

    /// <summary>
    /// Stops request handling by throwing an end-of-chain exception.
    /// </summary>
    /// <exception cref="HandlerReachedEndOfChainException">Always thrown to signal the end of the handler chain.</exception>
    [DoesNotReturn]
    internal static void StopHandling()
    {
        throw new HandlerReachedEndOfChainException();
    }

    /// <summary>
    /// Determines whether the specified exception indicates that chain execution should stop.
    /// </summary>
    /// <param name="e">The exception to inspect.</param>
    /// <returns><see langword="true"/> if execution should stop; otherwise, <see langword="false"/>.</returns>
    protected static bool ShouldStop(Exception e)
    {
        return e is HandlerReachedEndOfChainException;
    }
}
