namespace HedgeCraft.Elements.Testing.Shared;

using System;
using System.Linq;
using System.Runtime.InteropServices;

using TUnit.Core;

/// <summary>
/// Provides helper methods to conditionally execute tests on specific operating system platforms.
/// </summary>
internal static class RunTest
{
    /// <summary>
    /// Skips the test unless running on the specified operating system platform.
    /// </summary>
    /// <param name="osPlatform">The required operating system platform.</param>
    public static void OnlyOn(OSPlatform osPlatform)
    {
        Skip.Unless(RuntimeInformation.IsOSPlatform(osPlatform), $"This test is designed to run only on {osPlatform}, not on {Environment.OSVersion.Platform}");
    }

    /// <summary>
    /// Skips the test unless running on at least one of the specified operating system platforms.
    /// </summary>
    /// <param name="osPlatforms">The allowed operating system platforms.</param>
    public static void OnlyOn(OSPlatform[] osPlatforms)
    {
        Skip.Unless(osPlatforms.Any(RuntimeInformation.IsOSPlatform), $"This test is designed to run only on {string.Join(',', osPlatforms)}, not on {Environment.OSVersion.Platform}");
    }

    /// <summary>
    /// Skips the test when running on the specified operating system platform.
    /// </summary>
    /// <param name="osPlatform">The excluded operating system platform.</param>
    public static void NeverOn(OSPlatform osPlatform)
    {
        Skip.When(RuntimeInformation.IsOSPlatform(osPlatform),
            $"This test is not designed to run on {osPlatform}");
    }

    /// <summary>
    /// Skips the test when running on any of the specified operating system platforms.
    /// </summary>
    /// <param name="osPlatforms">The excluded operating system platforms.</param>
    public static void NeverOn(OSPlatform[] osPlatforms)
    {
        Skip.When(osPlatforms.Any(RuntimeInformation.IsOSPlatform),
            $"This test is not designed to run on {string.Join(',', osPlatforms)}");
    }
}
