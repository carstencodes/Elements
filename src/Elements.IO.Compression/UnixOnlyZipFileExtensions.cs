// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.IO;
using System.IO.Compression;
using System.Text;

namespace HedgeCraft.Elements.IO.Compression;

/// <summary>
/// Provides Unix-specific extension methods for <see cref="ZipArchive"/> to create symbolic links.
/// </summary>
public static class UnixOnlyZipFileExtensions
{
    /// <summary>
    /// Creates a new symbolic link entry in the zip archive pointing to the specified file.
    /// </summary>
    /// <param name="archive">The zip archive.</param>
    /// <param name="entryName">The name of the entry within the archive.</param>
    /// <param name="targetFile">The file info representing the link target.</param>
    /// <param name="currentEncoding">The character encoding for the target path, or <see langword="null"/> for default encoding.</param>
    /// <param name="level">The compression level.</param>
    /// <returns>The created <see cref="ZipArchiveEntry"/> representing the symbolic link.</returns>
    public static ZipArchiveEntry CreateSymbolicLink(this ZipArchive archive, string entryName, FileInfo targetFile, Encoding? currentEncoding = null, CompressionLevel level = default)
    {
        return archive.CreateSymbolicLink(targetFile.FullName, entryName, currentEncoding, level);
    }

    /// <summary>
    /// Creates a new symbolic link entry in the zip archive pointing to the specified target file path.
    /// </summary>
    /// <param name="archive">The zip archive.</param>
    /// <param name="entryName">The name of the entry within the archive.</param>
    /// <param name="targetFilePath">The target path the link points to.</param>
    /// <param name="currentEncoding">The character encoding for the target path, or <see langword="null"/> for default encoding.</param>
    /// <param name="level">The compression level.</param>
    /// <returns>The created <see cref="ZipArchiveEntry"/> representing the symbolic link.</returns>
    public static ZipArchiveEntry CreateSymbolicLink(this ZipArchive archive, string entryName, string targetFilePath, Encoding? currentEncoding = null, CompressionLevel level = default)
    {
        ZipArchiveEntry entry = archive.CreateEntry(entryName, level);
        entry.MakeSymbolicLinkTo(targetFilePath, currentEncoding);
        return entry;
    }
}
