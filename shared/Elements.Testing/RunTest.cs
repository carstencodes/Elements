namespace HedgeCraft.Elements.Testing.Shared;

using System;
using System.Linq;
using System.Runtime.InteropServices;

using TUnit.Core;

internal static class RunTest
{
    public static void OnlyOn(OSPlatform osPlatform)
    {
        Skip.Unless(RuntimeInformation.IsOSPlatform(osPlatform), $"This test is designed to run only on {osPlatform}, not on {Environment.OSVersion.Platform}");
    }

    public static void OnlyOn(OSPlatform[] osPlatforms)
    {
        Skip.Unless(osPlatforms.Any(RuntimeInformation.IsOSPlatform), $"This test is designed to run only on {string.Join(',', osPlatforms)}, not on {Environment.OSVersion.Platform}");
    }

    public static void NeverOn(OSPlatform osPlatform)
    {
        Skip.When(RuntimeInformation.IsOSPlatform(osPlatform),
            $"This test is not designed to run on {osPlatform}");
    }

    public static void NeverOn(OSPlatform[] osPlatforms)
    {
        Skip.When(osPlatforms.Any(RuntimeInformation.IsOSPlatform),
            $"This test is not designed to run on {string.Join(',', osPlatforms)}");
    }
}
