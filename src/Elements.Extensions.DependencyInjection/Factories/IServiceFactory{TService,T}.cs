// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Diagnostics.CodeAnalysis;

namespace HedgeCraft.Elements.Extensions.DependencyInjection.Factories;

/// <summary>
/// Defines a factory for creating service instances of type <typeparamref name="TService"/> with one argument.
/// </summary>
/// <typeparam name="TService">The type of service to create.</typeparam>
/// <typeparam name="T">The type of the first argument.</typeparam>
public interface IServiceFactory<out TService, in T>
{
    /// <summary>
    /// Creates a new service instance of type <typeparamref name="TService"/>.
    /// </summary>
    /// <param name="arg">The argument used for creating the service.</param>
    /// <returns>A new instance of <typeparamref name="TService"/>.</returns>
    TService CreateInstance(T arg);
}
