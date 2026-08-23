// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace HedgeCraft.Elements.IO.Compression;

/// <summary>
/// Provides Unix-specific extension methods for <see cref="ZipArchiveEntry"/> to support symbolic links.
/// </summary>
public static class UnixOnlyZipEntryExtensions
{
    private const int SymbolicLinkMarkerFlag = 0xA000;
    private const int ByteShiftToShort = 16;

    /// <summary>
    /// Determines whether the specified zip entry represents a symbolic link.
    /// </summary>
    /// <param name="entry">The zip archive entry to inspect.</param>
    /// <returns><see langword="true"/> if the entry is a symbolic link; otherwise, <see langword="false"/>.</returns>
    public static bool IsSymbolicLink(this ZipArchiveEntry entry)
    {
        int attributes = entry.ExternalAttributes;
        attributes >>= ByteShiftToShort;

        return (attributes & SymbolicLinkMarkerFlag) == SymbolicLinkMarkerFlag;
    }

    /// <summary>
    /// Configures the zip archive entry as a symbolic link pointing to the specified file.
    /// </summary>
    /// <param name="entry">The zip archive entry to configure.</param>
    /// <param name="targetFile">The file info representing the link target.</param>
    /// <param name="currentEncoding">The encoding used for writing the target path, or <see langword="null"/> for default encoding.</param>
    public static void MakeSymbolicLinkTo(this ZipArchiveEntry entry, FileInfo targetFile, Encoding? currentEncoding = null)
    {
        entry.MakeSymbolicLinkTo(targetFile.FullName, currentEncoding);
    }

    /// <summary>
    /// Configures the zip archive entry as a symbolic link pointing to the specified target path.
    /// </summary>
    /// <param name="entry">The zip archive entry to configure.</param>
    /// <param name="targetFilePath">The target path the link points to.</param>
    /// <param name="currentEncoding">The encoding used for writing the target path, or <see langword="null"/> for default encoding.</param>
    public static void MakeSymbolicLinkTo(this ZipArchiveEntry entry, string targetFilePath, Encoding? currentEncoding = null)
    {
        UnixFileMode fileMode = UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute
                                | UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.GroupExecute
                                | UnixFileMode.OtherRead | UnixFileMode.OtherWrite | UnixFileMode.OtherExecute;
        int attributes = (int)fileMode;
        attributes += SymbolicLinkMarkerFlag;
        attributes <<= ByteShiftToShort;

        entry.ExternalAttributes = attributes;
        currentEncoding ??= Encoding.Default;

        ReadOnlyMemory<byte> bytes = currentEncoding.GetBytes(targetFilePath);
        using Stream stream = entry.Open();
        stream.Write(bytes.Span);
        stream.Close();
    }
}
