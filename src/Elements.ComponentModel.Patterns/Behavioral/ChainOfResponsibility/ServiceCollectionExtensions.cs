using System;
using System.Collections.Generic;
using System.Linq;
using HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseChainOfResponsibility<TService, TServiceHandlerImpl>(this IServiceCollection services,
        Func<TService, Action> handlerFunctionFactory,
        Func<TService, Func<bool>> canHandleFunctionFactory,
        Func<Handler, TServiceHandlerImpl> serviceFromHandlerFactory,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : notnull
            where TServiceHandlerImpl : TService
    {
        Handler HandlerFromServices(IEnumerable<TService> serviceInstances)
        {
            HandlerChainLink current = HandlerChainLink.None;
            foreach (TService service in serviceInstances.Reverse())
            {
                current = new HandlerChainLink(
                    handlerFunctionFactory(service),
                    canHandleFunctionFactory(service),
                    current
                );
            }

            return new Handler(current);
        }

        TServiceHandlerImpl CreateNewService(IServiceProvider serviceProvider)
        {
            IEnumerable<TService> createdServices = serviceProvider.GetRequiredService<IEnumerable<TService>>();
            Handler handler = HandlerFromServices(createdServices);
            return serviceFromHandlerFactory(handler);
        }

        Func<IServiceProvider, object> concreteFactory = sp => CreateNewService(sp);

        services.Add(new ServiceDescriptor(typeof(TServiceHandlerImpl), concreteFactory, lifetime));

        return services;
    }

    public static IServiceCollection UseChainOfResponsibility<TService, TServiceHandlerImpl, TResult>(this IServiceCollection services,
        Func<TService, Func<TResult>> handlerFunctionFactory,
        Func<TService, Func<bool>> canHandleFunctionFactory,
        Func<ResultedHandler<TResult>, TServiceHandlerImpl> serviceFromHandlerFactory,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TService : notnull
        where TServiceHandlerImpl : TService
        where TResult : notnull
    {
        ResultedHandler<TResult> HandlerFromServices(IEnumerable<TService> serviceInstances)
        {
            HandlerChainLink<TResult> current = HandlerChainLink<TResult>.None;
            foreach (TService service in serviceInstances.Reverse())
            {
                current = new HandlerChainLink<TResult>(
                    handlerFunctionFactory(service),
                    canHandleFunctionFactory(service),
                    current
                );
            }

            return new ResultedHandler<TResult>(current);
        }

        TServiceHandlerImpl CreateNewService(IServiceProvider serviceProvider)
        {
            IEnumerable<TService> createdServices = serviceProvider.GetRequiredService<IEnumerable<TService>>();
            ResultedHandler<TResult> handler = HandlerFromServices(createdServices);
            return serviceFromHandlerFactory(handler);
        }

        Func<IServiceProvider, object> concreteFactory = sp => CreateNewService(sp);

        services.Add(new ServiceDescriptor(typeof(TServiceHandlerImpl), concreteFactory, lifetime));

        return services;
    }
}
