// SPDX-Identifier: MIT
//
//  (C) 2023-2026 Carsten Igel.
//  Published under MIT License

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

/// <summary>
/// Provides extension methods to facilitate working with
/// handler chain links in the Chain of Responsibility pattern.
/// </summary>
internal static class HandlerChainLinkExtensions
{
    extension<TSelf>(IHandler<TSelf> handler) where TSelf : IHandler<TSelf>
    {
        /// <summary>
        /// Determines whether the current handler link represents a terminal or uninitialized state in the chain.
        /// </summary>
        /// <returns>
        /// A boolean value indicating whether the current handler link is considered "none."
        /// </returns>
        internal bool IsNone()
        {
            HandlerChainLink none = new();
            return handler.Equals(none);
        }
    }

    extension<TSelf, TResult>(IHandler<TSelf, TResult> handler) where TSelf : IHandler<TSelf, TResult> where TResult : notnull
    {
        /// <summary>
        /// Determines whether the current handler link represents a terminal or uninitialized state in the chain.
        /// </summary>
        /// <returns>
        /// A boolean value indicating whether the current handler link is considered "none."
        /// </returns>
        internal bool IsNone()
        {
            HandlerChainLink<TResult> none = new();
            return handler.Equals(none);
        }
    }
}
