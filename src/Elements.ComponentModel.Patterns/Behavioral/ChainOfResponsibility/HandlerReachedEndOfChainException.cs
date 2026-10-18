// SPDX-Identifier: MIT
//
//  (C) 2023-2026 Carsten Igel.
//  Published under MIT License

using System;
using System.Diagnostics.CodeAnalysis;

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

/// <summary>
/// Represents the exception thrown when a request reaches the end of the chain of responsibility without being handled.
/// </summary>
public sealed class HandlerReachedEndOfChainException : Exception
{
    private const string DefaultMessage = "The handler has no other handler in its chain";

    /// <summary>
    /// Initializes a new instance of the <see cref="HandlerReachedEndOfChainException"/> class.
    /// </summary>
    public HandlerReachedEndOfChainException() : base(DefaultMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HandlerReachedEndOfChainException"/> class with a message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public HandlerReachedEndOfChainException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HandlerReachedEndOfChainException"/> class with a message and an inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public HandlerReachedEndOfChainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
