using System;

namespace HedgeCraft.Elements.Testing.Shared;

using System.Linq;
using System.Runtime.InteropServices;

using TUnit.Core;


internal static class SkipTest
{
    public static void IfRunningOn(OSPlatform osPlatform)
    {
        Skip.When(RuntimeInformation.IsOSPlatform(osPlatform),
            $"This test is not designed to run on {osPlatform}");
    }

    public static void IfRunningOn(OSPlatform[] osPlatforms)
    {
        Skip.When(osPlatforms.Any(RuntimeInformation.IsOSPlatform),
            $"This test is not designed to run on {string.Join(',', osPlatforms)}");
    }

    public static void IfNotRunningOn(OSPlatform osPlatform)
    {
        Skip.Unless(RuntimeInformation.IsOSPlatform(osPlatform),
            $"This test is designed to run on {osPlatform}, not on {Environment.OSVersion.Platform}");
    }

    public static void IfNotRunningOn(OSPlatform[] osPlatforms)
    {
        Skip.Unless(osPlatforms.Any(RuntimeInformation.IsOSPlatform),
            $"This test is designed to run on {string.Join(',', osPlatforms)}, not on {Environment.OSVersion.Platform}");
    }
}
