using System;
using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.DependencyInjection;

public static class ServiceScopeExtensions
{
    public static TService CreateDynamicInstance<TService, T1>(this IServiceScope scope, T1 arg1)
        where TService : notnull
        where T1 : notnull
    {
        Func<T1, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1>();
        return serviceFactory(arg1);
    }

    public static TService CreateDynamicInstance<TService, T1, T2>(this IServiceScope scope, T1 arg1, T2 arg2)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
    {
        Func<T1, T2, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2>();
        return serviceFactory(arg1, arg2);
    }

    public static TService CreateDynamicInstance<TService, T1, T2, T3>(this IServiceScope scope, T1 arg1, T2 arg2, T3 arg3)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
    {
        Func<T1, T2, T3, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2, T3>();
        return serviceFactory(arg1, arg2, arg3);
    }

    public static TService CreateDynamicInstance<TService, T1, T2, T3, T4>(this IServiceScope scope, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
    {
        Func<T1, T2, T3, T4, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2, T3, T4>();
        return serviceFactory(arg1, arg2, arg3, arg4);
    }

    public static TService CreateDynamicInstance<TService, T1, T2, T3, T4, T5>(this IServiceScope scope, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
    {
        Func<T1, T2, T3, T4, T5, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2, T3, T4, T5>();
        return serviceFactory(arg1, arg2, arg3, arg4, arg5);
    }

    public static TService CreateDynamicInstance<TService, T1, T2, T3, T4, T5, T6>(this IServiceScope scope, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
    {
        Func<T1, T2, T3, T4, T5, T6, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2, T3, T4, T5, T6>();
        return serviceFactory(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    public static TService CreateDynamicInstance<TService, T1, T2, T3, T4, T5, T6, T7>(this IServiceScope scope, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
    {
        Func<T1, T2, T3, T4, T5, T6, T7, TService> serviceFactory = scope.ServiceProvider.FindRequiredServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7>();
        return serviceFactory(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }
}
