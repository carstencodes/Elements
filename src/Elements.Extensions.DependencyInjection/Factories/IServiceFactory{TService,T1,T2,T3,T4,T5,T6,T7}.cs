// SPDX-Identifier: MIT
//
//  (C) 2023-2026 Carsten Igel.
//  Published under MIT License

namespace HedgeCraft.Elements.Extensions.DependencyInjection.Factories;

/// <summary>
/// Defines a factory for creating service instances of type <typeparamref name="TService"/> with seven arguments.
/// </summary>
/// <typeparam name="TService">The type of service to create.</typeparam>
/// <typeparam name="T1">The type of the first argument.</typeparam>
/// <typeparam name="T2">The type of the second argument.</typeparam>
/// <typeparam name="T3">The type of the third argument.</typeparam>
/// <typeparam name="T4">The type of the fourth argument.</typeparam>
/// <typeparam name="T5">The type of the fifth argument.</typeparam>
/// <typeparam name="T6">The type of the sixth argument.</typeparam>
/// <typeparam name="T7">The type of the seventh argument.</typeparam>
public interface IServiceFactory<out TService, in T1, in T2, in T3, in T4, in T5, in T6, in T7>
{
    /// <summary>
    /// Creates a new service instance of type <typeparamref name="TService"/>.
    /// </summary>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth argument.</param>
    /// <param name="arg6">The sixth argument.</param>
    /// <param name="arg7">The seventh argument.</param>
    /// <returns>A new instance of <typeparamref name="TService"/>.</returns>
    TService CreateInstance(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
}
