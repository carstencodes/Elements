using System.Diagnostics.CodeAnalysis;

namespace Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

public abstract class HandlerBase
{
    private sealed class HandlerReachedEndOfChainException()
        : Exception("The handler has no other handler in its chain");
    
    [DoesNotReturn]internal static void StopHandling()
    {
        throw new HandlerReachedEndOfChainException();
    }

    protected static bool ShouldStop(Exception e)
    {
        return e is HandlerReachedEndOfChainException;
    }
}