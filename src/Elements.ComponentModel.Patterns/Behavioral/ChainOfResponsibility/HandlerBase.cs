using System;
using System.Diagnostics.CodeAnalysis;

namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

public abstract class HandlerBase
{
    public sealed class HandlerReachedEndOfChainException(Exception innerException)
        : Exception("The handler has no other handler in its chain", innerException)
    {
        public HandlerReachedEndOfChainException(): this(null!)
        {
        }
    }

    [DoesNotReturn]
    internal static void StopHandling()
    {
        throw new HandlerReachedEndOfChainException();
    }

    protected static bool ShouldStop(Exception e)
    {
        return e is HandlerReachedEndOfChainException;
    }
}
