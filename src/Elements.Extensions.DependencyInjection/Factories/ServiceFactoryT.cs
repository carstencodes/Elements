using System;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.Factories;

internal sealed class ServiceFactory<TService, T1> : IServiceFactory<TService, T1>
{
    private readonly IServiceProvider provider;

    private readonly Func<IServiceProvider, T1, TService> factory;
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    public TService CreateInstance(T1 arg1)
    {
        return this.factory(this.provider, arg1);
    }
}

internal sealed class ServiceFactory<TService, T1, T2> : IServiceFactory<TService, T1, T2>
{
    private readonly IServiceProvider provider;

    private readonly Func<IServiceProvider, T1, T2, TService> factory;
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    public TService CreateInstance(T1 arg1, T2 arg2)
    {
        return this.factory(this.provider, arg1, arg2);
    }
}

internal sealed class ServiceFactory<TService, T1, T2, T3> : IServiceFactory<TService, T1, T2, T3>
{
    private readonly IServiceProvider provider;

    private readonly Func<IServiceProvider, T1, T2, T3, TService> factory;
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, T3, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    public TService CreateInstance(T1 arg1, T2 arg2, T3 arg3)
    {
        return this.factory(this.provider, arg1, arg2, arg3);
    }
}

internal sealed class ServiceFactory<TService, T1, T2, T3, T4> : IServiceFactory<TService, T1, T2, T3, T4>
{
    private readonly IServiceProvider provider;

    private readonly Func<IServiceProvider, T1, T2, T3, T4, TService> factory;
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, T3, T4, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    public TService CreateInstance(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return this.factory(this.provider, arg1, arg2, arg3, arg4);
    }
}

internal sealed class ServiceFactory<TService, T1, T2, T3, T4, T5> : IServiceFactory<TService, T1, T2, T3, T4, T5>
{
    private readonly IServiceProvider provider;

    private readonly Func<IServiceProvider, T1, T2, T3, T4, T5, TService> factory;
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, T3, T4, T5, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    public TService CreateInstance(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return this.factory(this.provider, arg1, arg2, arg3, arg4, arg5);
    }
}

internal sealed class ServiceFactory<TService, T1, T2, T3, T4, T5, T6> : IServiceFactory<TService, T1, T2, T3, T4, T5, T6>
{
    private readonly IServiceProvider provider;

    private readonly Func<IServiceProvider, T1, T2, T3, T4, T5, T6, TService> factory;
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, T3, T4, T5, T6, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    public TService CreateInstance(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return this.factory(this.provider, arg1, arg2, arg3, arg4, arg5, arg6);
    }
}

internal sealed class ServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7> : IServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7>
{
    private readonly IServiceProvider provider;

    private readonly Func<IServiceProvider, T1, T2, T3, T4, T5, T6, T7, TService> factory;
    public ServiceFactory(IServiceProvider provider, Func<IServiceProvider, T1, T2, T3, T4, T5, T6, T7, TService> factory)
    {
        this.provider = provider;
        this.factory = factory;
    }

    public TService CreateInstance(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return this.factory(this.provider, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }
}
