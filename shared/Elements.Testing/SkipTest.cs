namespace HedgeCraft.Elements.Testing.Shared;

using System;
using System.Linq;
using System.Runtime.InteropServices;

using TUnit.Core;

/// <summary>
/// Provides helper methods to conditionally skip tests based on operating system platforms.
/// </summary>
internal static class SkipTest
{
    /// <summary>
    /// Skips the test if executing on the specified operating system platform.
    /// </summary>
    /// <param name="osPlatform">The operating system platform on which the test should skip.</param>
    public static void IfRunningOn(OSPlatform osPlatform)
    {
        Skip.When(RuntimeInformation.IsOSPlatform(osPlatform),
            $"This test is not designed to run on {osPlatform}");
    }

    /// <summary>
    /// Skips the test if executing on any of the specified operating system platforms.
    /// </summary>
    /// <param name="osPlatforms">The operating system platforms on which the test should skip.</param>
    public static void IfRunningOn(OSPlatform[] osPlatforms)
    {
        Skip.When(osPlatforms.Any(RuntimeInformation.IsOSPlatform),
            $"This test is not designed to run on {string.Join(',', osPlatforms)}");
    }

    /// <summary>
    /// Skips the test if not executing on the specified operating system platform.
    /// </summary>
    /// <param name="osPlatform">The required operating system platform.</param>
    public static void IfNotRunningOn(OSPlatform osPlatform)
    {
        Skip.Unless(RuntimeInformation.IsOSPlatform(osPlatform),
            $"This test is designed to run on {osPlatform}, not on {Environment.OSVersion.Platform}");
    }

    /// <summary>
    /// Skips the test if not executing on at least one of the specified operating system platforms.
    /// </summary>
    /// <param name="osPlatforms">The required operating system platforms.</param>
    public static void IfNotRunningOn(OSPlatform[] osPlatforms)
    {
        Skip.Unless(osPlatforms.Any(RuntimeInformation.IsOSPlatform),
            $"This test is designed to run on {string.Join(',', osPlatforms)}, not on {Environment.OSVersion.Platform}");
    }
}
