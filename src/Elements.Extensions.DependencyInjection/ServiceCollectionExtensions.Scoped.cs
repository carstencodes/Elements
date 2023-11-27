using Microsoft.Extensions.DependencyInjection;

namespace Elements.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static void RegisterScopedFactory<TService, T1>(this IServiceCollection services) 
        where TService : notnull
        where T1: notnull
    {
    }
}