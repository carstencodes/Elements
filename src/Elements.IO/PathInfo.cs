// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections.Generic;
using System.IO;

namespace HedgeCraft.Elements.IO;

/// <summary>
/// Represents a unified path abstraction that can identify a file, a directory, or a drive root.
/// </summary>
public sealed class PathInfo : FileSystemInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PathInfo"/> class from the specified path string.
    /// </summary>
    /// <param name="path">The file system path.</param>
    public PathInfo(string path)
    {
        this.FullPath = Path.GetFullPath(path);
        string name = Path.GetFileName(this.FullPath);
        if (string.IsNullOrWhiteSpace(name))
        {
            name = Path.GetDirectoryName(this.FullPath) ?? path;
        }

        this.Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PathInfo"/> class from an existing <see cref="FileSystemInfo"/>.
    /// </summary>
    /// <param name="fileSystemEntry">The existing file system info entry.</param>
    public PathInfo(FileSystemInfo fileSystemEntry)
    {
        this.FullPath = fileSystemEntry.FullName;
        this.Name = fileSystemEntry.Name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PathInfo"/> class from a <see cref="DriveInfo"/> root.
    /// </summary>
    /// <param name="rootDrive">The drive information.</param>
    public PathInfo(DriveInfo rootDrive)
    {
        this.FullPath = rootDrive.RootDirectory.FullName;
        this.Name = rootDrive.Name;
    }

    /// <summary>
    /// Gets a value indicating whether this path refers to an existing file.
    /// </summary>
    public bool IsFile
    {
        get
        {
            return File.Exists(this.FullName);
        }
    }

    /// <summary>
    /// Gets a value indicating whether this path refers to an existing directory.
    /// </summary>
    public bool IsDirectory
    {
        get
        {
            return Directory.Exists(this.FullName);
        }
    }

    /// <summary>
    /// Gets a value indicating whether this path exists on disk as either a file or a directory.
    /// </summary>
    public override bool Exists
    {
        get
        {
            return this.IsFile || this.IsDirectory;
        }
    }

    /// <summary>
    /// Gets the name of the file or directory.
    /// </summary>
    public override string Name { get; }

    /// <summary>
    /// Deletes the file or directory represented by this path if it exists.
    /// </summary>
    public override void Delete()
    {
        if (this.Exists)
        {
            if (this.IsFile)
            {
                File.Delete(this.FullName);
            }
            else if (this.IsDirectory)
            {
                Directory.Delete(this.FullName, Directory.GetFileSystemEntries(this.FullName).Length > 0);
            }
        }
    }

    /// <summary>
    /// Combines the current path with one or more additional path segments.
    /// </summary>
    /// <param name="pathParts">The relative path segments to append.</param>
    /// <returns>A new <see cref="PathInfo"/> representing the combined path.</returns>
    public PathInfo Combine(params string[] pathParts)
    {
        List<string> parts =
        [
            this.FullName
        ];
        parts.AddRange(pathParts);
        string finalPath = Path.Combine(parts.ToArray());
        return new PathInfo(finalPath);
    }

    /// <summary>
    /// Combines a parent <see cref="PathInfo"/> with a child path segment using the division operator.
    /// </summary>
    /// <param name="path">The base path info.</param>
    /// <param name="child">The relative child path segment to append.</param>
    /// <returns>A new <see cref="PathInfo"/> representing the combined path.</returns>
    public static PathInfo operator /(PathInfo path, string child)
    {
        return Divide(path, child);
    }

    /// <summary>
    /// Returns a <see cref="FileInfo"/> representing the current path.
    /// </summary>
    /// <returns>A new <see cref="FileInfo"/> instance.</returns>
    public FileInfo AsFile()
    {
        return new(this.FullName);
    }

    /// <summary>
    /// Returns a <see cref="DirectoryInfo"/> representing the current path.
    /// </summary>
    /// <returns>A new <see cref="DirectoryInfo"/> instance.</returns>
    public DirectoryInfo AsDirectory()
    {
        return new(this.FullName);
    }

    /// <summary>
    /// Combines a base <see cref="PathInfo"/> with a child path segment.
    /// </summary>
    /// <param name="path">The base path info.</param>
    /// <param name="child">The child path segment to append.</param>
    /// <returns>A new <see cref="PathInfo"/> representing the combined path.</returns>
    public static PathInfo Divide(PathInfo path, string child)
    {
        return path.Combine(child);
    }
}
