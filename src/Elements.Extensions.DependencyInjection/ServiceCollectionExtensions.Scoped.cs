using HedgeCraft.Elements.Extensions.DependencyInjection.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace HedgeCraft.Elements.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterScopedFactory<TService, T1>(this IServiceCollection services, Func<IServiceProvider, T1, TService> factory) 
        where TService : notnull
        where T1: notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1>, ServiceFactory<TService, T1>>();
    }
    
    public static IServiceCollection RegisterScopedFactory<TService, T1, T2>(this IServiceCollection services, Func<IServiceProvider, T1, T2, TService> factory) 
        where TService : notnull
        where T1: notnull
        where T2: notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2>, ServiceFactory<TService, T1, T2>>();
    }
    
    public static IServiceCollection RegisterScopedFactory<TService, T1, T2, T3>(this IServiceCollection services, Func<IServiceProvider, T1, T2, T3, TService> factory) 
        where TService : notnull
        where T1: notnull
        where T2 : notnull
        where T3 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2, T3>, ServiceFactory<TService, T1, T2, T3>>();
    }
    
    public static IServiceCollection RegisterScopedFactory<TService, T1, T2, T3, T4>(this IServiceCollection services, Func<IServiceProvider, T1, T2, T3, T4, TService> factory) 
        where TService : notnull
        where T1: notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2, T3, T4>, ServiceFactory<TService, T1, T2, T3, T4>>();
    }
    
    public static IServiceCollection RegisterScopedFactory<TService, T1, T2, T3, T4, T5>(this IServiceCollection services, Func<IServiceProvider, T1, T2, T3, T4, T5, TService> factory) 
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2, T3, T4, T5>, ServiceFactory<TService, T1, T2, T3, T4, T5>>();
    }
    
    public static IServiceCollection RegisterScopedFactory<TService, T1, T2, T3, T4, T5, T6>(this IServiceCollection services, Func<IServiceProvider, T1, T2, T3, T4, T5, T6, TService> factory) 
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2, T3, T4, T5, T6>, ServiceFactory<TService, T1, T2, T3, T4, T5, T6>>();
    }

    public static IServiceCollection RegisterScopedFactory<TService, T1, T2, T3, T4, T5, T6, T7>(
        this IServiceCollection services, Func<IServiceProvider, T1, T2, T3, T4, T5, T6, T7, TService> factory)
        where TService : notnull
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
    {
        return services
            .AddSingleton(factory)
            .AddScoped<IServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7>, ServiceFactory<TService, T1, T2, T3, T4, T5, T6, T7>>();
    }
}