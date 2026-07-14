// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace HedgeCraft.Elements.IO.Compression;

public static class UnixOnlyZipEntryExtensions
{
    private const int SymbolicLinkMarkerFlag = 0xA000;
    private const int ByteShiftToShort = 16;

    public static bool IsSymbolicLink(this ZipArchiveEntry entry)
    {
        int attributes = entry.ExternalAttributes;
        attributes >>= ByteShiftToShort;

        return (attributes & SymbolicLinkMarkerFlag) == SymbolicLinkMarkerFlag;
    }

    public static void MakeSymbolicLinkTo(this ZipArchiveEntry entry, FileInfo targetFile, Encoding? currentEncoding = null)
    {
        entry.MakeSymbolicLinkTo(targetFile.FullName, currentEncoding);
    }

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
