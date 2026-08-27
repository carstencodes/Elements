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
        [<target-dir>]       Path to the target directory to install to.

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
string targetDir;
if (args.Length == 2)
{
    targetDir = args[1];
}
else
{
    targetDir = string.Empty;
}

if (!File.Exists(iniPath))
{
    await Console.Error.WriteLineAsync($"File not found: {iniPath}");
    return 1;
}

if (!string.IsNullOrWhiteSpace(targetDir) && !Directory.Exists(targetDir))
{
    DirectoryInfo? currentDir = new DirectoryInfo(targetDir);
    Stack<DirectoryInfo> toCreate = new();
    while (currentDir is not null and not { Exists: true })
    {
        toCreate.Push(currentDir);
        currentDir = currentDir.Parent;
    }

    while (toCreate.Count > 0)
    {
        DirectoryInfo dirToCreate = toCreate.Pop();
        Console.WriteLine($"Creating directory: {dirToCreate.FullName}");
        dirToCreate.Create();
    }
}

List<string> entries = (await File.ReadAllLinesAsync(iniPath))
    // skip empty lines
    .Where(line => !string.IsNullOrWhiteSpace(line))
    // skip section headers: ^\[
    .Where(line => !line.TrimStart().StartsWith('['))
#pragma warning disable S125
    // skip comments: ^;
#pragma warning restore S125
    .Where(line => !line.TrimStart().StartsWith(';'))
    // sed 's/=/@/g'
    .Select(line => line.Replace('=', '@'))
    // unquoted $(...) word-splits on whitespace too
    .SelectMany(line => line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
    .ToList();

foreach (string entry in entries)
{
#pragma warning disable S4036
    ProcessStartInfo psi = new("cargo")
    {
        UseShellExecute = false,
    };
#pragma warning restore S4036
    psi.ArgumentList.Add("install");
    psi.ArgumentList.Add("--locked");
    if (!string.IsNullOrWhiteSpace(targetDir))
    {
        psi.ArgumentList.Add("--root");
        psi.ArgumentList.Add(targetDir);
    }
    psi.ArgumentList.Add(entry);

    Console.WriteLine($"cargo {string.Join(" ", psi.ArgumentList)}");

    using Process? process = Process.Start(psi);
    if (process is not null)
    {
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            await Console.Error.WriteLineAsync($"cargo install failed for '{entry}' (exit code {process.ExitCode})");
        }
    }
    else
    {
        await Console.Error.WriteLineAsync($"Failed to start 'cargo install --locked {entry}'");
    }
}

return 0;
