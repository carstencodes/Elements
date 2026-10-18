// SPDX-Identifier: MIT
//
//  (C) 2023-2026 Carsten Igel.
//  Published under MIT License

namespace HedgeCraft.Elements.Extensions.DependencyInjection.Factories;

/// <summary>
/// Defines a factory for creating service instances of type <typeparamref name="TService"/> with two arguments.
/// </summary>
/// <typeparam name="TService">The type of service to create.</typeparam>
/// <typeparam name="T1">The type of the first argument.</typeparam>
/// <typeparam name="T2">The type of the second argument.</typeparam>
public interface IServiceFactory<out TService, in T1, in T2>
{
    /// <summary>
    /// Creates a new service instance of type <typeparamref name="TService"/>.
    /// </summary>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <returns>A new instance of <typeparamref name="TService"/>.</returns>
    TService CreateInstance(T1 arg1, T2 arg2);
}
