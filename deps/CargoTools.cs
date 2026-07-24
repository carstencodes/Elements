#!/usr/bin/env -S dotnet --

#:property TargetFramework=net10.0
#:property PublishAot=false
#:property GenerateDocumentationFile=false
#:property EnableNETAnalyzers=false
#:property SkipCargoSetVersion=true
#:property SkipNuGetLicense=true
#:property GenerateCycloneDxSbom=false

#pragma warning disable MA0029
#pragma warning disable MA0042
#pragma warning disable MA0048
#pragma warning disable MA0076

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

void PrintUsage()
{
    Console.WriteLine("""
        Usage: dotnet run CargoTools.cs <path-to-ini-file>

        Reads a cargo-tools style .ini file and runs `cargo install --locked <pkg@version>`
        for every entry, skipping section headers (lines starting with '[') and
        comments (lines starting with ';').

        Example ini format:
            [tools]
            ripgrep=14.1.0
            fd-find=10.2.0
            ;this-is-commented-out=1.0.0

        Arguments:
          <path-to-ini-file>   Path to the ini file listing packages to install.

        Options:
          -h, --help           Show this usage text and exit.
        """);
}

if (args.Length == 0 || args[0] is "-h" or "--help")
{
    PrintUsage();
    return args.Length == 0 ? 1 : 0;
}

string iniPath = args[0];

if (!File.Exists(iniPath))
{
    Console.Error.WriteLine($"File not found: {iniPath}");
    return 1;
}

List<string> entries = File.ReadAllLines(iniPath)
    // skip empty lines
    .Where(line => !string.IsNullOrWhiteSpace(line))
    // skip section headers: ^\[
    .Where(line => !line.TrimStart().StartsWith('['))
    // skip comments: ^;
    .Where(line => !line.TrimStart().StartsWith(';'))
    // sed 's/=/@/g'
    .Select(line => line.Replace('=', '@'))
    // unquoted $(...) word-splits on whitespace too
    .SelectMany(line => line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
    .ToList();

foreach (string entry in entries)
{
    Console.WriteLine($"cargo install --locked {entry}");

    ProcessStartInfo psi = new("cargo")
    {
        UseShellExecute = false,
    };
    psi.ArgumentList.Add("install");
    psi.ArgumentList.Add("--locked");
    psi.ArgumentList.Add(entry);

    using Process? process = Process.Start(psi);
    if (process is not  null)
    {
        process!.WaitForExit();

        if (process.ExitCode != 0)
        {
            Console.Error.WriteLine($"cargo install failed for '{entry}' (exit code {process.ExitCode})");
        }
    }
    else
    {
        Console.Error.WriteLine($"Failed to start 'cargo install --locked {entry}'");
    }
}

return 0;
