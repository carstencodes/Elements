using System.IO;
using System.IO.Compression;
using System.Text;

namespace HedgeCraft.Elements.IO.Compression;

public static class UnixOnlyZipFileExtensions
{
    public static ZipArchiveEntry CreateSymbolicLink(this ZipArchive archive, string entryName, FileInfo targetFile, Encoding? currentEncoding = null, CompressionLevel level = default)
    {
        return archive.CreateSymbolicLink(targetFile.FullName, entryName, currentEncoding, level);
    }
    
    public static ZipArchiveEntry CreateSymbolicLink(this ZipArchive archive, string entryName, string targetFilePath, Encoding? currentEncoding = null, CompressionLevel level = default)
    {
        ZipArchiveEntry entry = archive.CreateEntry(entryName, level);
        entry.MakeSymbolicLinkTo(targetFilePath, currentEncoding);
        return entry;
    }
}