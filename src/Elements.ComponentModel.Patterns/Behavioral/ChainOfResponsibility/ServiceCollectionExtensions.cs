// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using HedgeCraft.Elements.ComponentModel.Patterns.Behavioral.ChainOfResponsibility;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for configuring chain of responsibility handlers in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a service implementation that dispatches requests across a chain of responsibility without a return value.
    /// </summary>
    /// <typeparam name="TService">The base service type represented by chain members.</typeparam>
    /// <typeparam name="TServiceHandlerImpl">The implementation type created from the chain handler.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="handlerFunctionFactory">A factory extracting the execution action from each service instance.</param>
    /// <param name="canHandleFunctionFactory">A factory extracting the predicate determining whether a service can handle the request.</param>
    /// <param name="serviceFromHandlerFactory">A factory creating the implementation instance given the coordinated handler.</param>
    /// <param name="lifetime">The service lifetime for the registered implementation.</param>
    /// <returns>The service collection instance.</returns>
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

    /// <summary>
    /// Registers a service implementation that dispatches requests across a chain of responsibility, returning a result.
    /// </summary>
    /// <typeparam name="TService">The base service type represented by chain members.</typeparam>
    /// <typeparam name="TServiceHandlerImpl">The implementation type created from the chain handler.</typeparam>
    /// <typeparam name="TResult">The result type produced by handling the request.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="handlerFunctionFactory">A factory extracting the result-producing function from each service instance.</param>
    /// <param name="canHandleFunctionFactory">A factory extracting the predicate determining whether a service can handle the request.</param>
    /// <param name="serviceFromHandlerFactory">A factory creating the implementation instance given the coordinated handler.</param>
    /// <param name="lifetime">The service lifetime for the registered implementation.</param>
    /// <returns>The service collection instance.</returns>
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
