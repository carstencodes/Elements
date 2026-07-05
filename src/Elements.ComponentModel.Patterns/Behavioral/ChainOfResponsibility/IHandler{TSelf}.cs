namespace HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

public interface IHandler<out TSelf> where TSelf: IHandler<TSelf>
{
    bool CanHandleCurrentRequest();

    void HandleCurrentRequest();
    
    IHandler<TSelf>? Next { get; }
}

public interface IHandler<out TSelf, out TResult> where TSelf: IHandler<TSelf, TResult> where TResult: notnull
{
    bool CanHandleCurrentRequest();

    TResult HandleCurrentRequest();
    
    IHandler<TSelf, TResult>? Next { get; }
}