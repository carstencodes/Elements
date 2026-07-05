using System.Collections.Generic;
using System.IO;

namespace HedgeCraft.Elements.IO;

public sealed class PathInfo: FileSystemInfo
{
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
    
    public PathInfo(FileSystemInfo fileSystemEntry)
    {
        this.FullPath = fileSystemEntry.FullName;
        this.Name = fileSystemEntry.Name;
    }
    
    public PathInfo(DriveInfo rootDrive)
    {
        this.FullPath = rootDrive.RootDirectory.FullName;
        this.Name = rootDrive.Name;
    }

    public bool IsFile
    {
        get
        {
            return File.Exists(this.FullName);
        }
    }
    
    public bool IsDirectory
    {
        get
        {
            return Directory.Exists(this.FullName);
        }
    }

    public override bool Exists
    {
        get
        {
            return this.IsFile || this.IsDirectory;
        }
    }

    public override string Name { get; }
    
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

    public static PathInfo operator /(PathInfo path, string child)
    {
        return path.Combine(child);
    }

    public FileInfo AsFile()
    {
        return new(this.FullName);
    }

    public DirectoryInfo AsDirectory()
    {
        return new(this.FullName);
    }
}