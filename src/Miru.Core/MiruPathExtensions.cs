using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Miru.Core;

public static class MiruPathExtensions
{
    public static bool FileExists(this MiruPath path) => File.Exists(path);
        
    public static bool FileDoesNotExist(this MiruPath path) => File.Exists(path) == false;
        
    public static string Relative(this MiruPath current, string path) => Path.GetRelativePath(current, path);
        
    public static void DeleteFileIfExists(this MiruPath miruPath) => Core.Files.DeleteIfExists(miruPath.ToString());
    
    public static void DeleteDir(this MiruPath miruPath) => Directories.DeleteIfExists(miruPath);

    public static bool DirExists(this MiruPath miruPath) =>
        Directory.Exists(miruPath);

    public static bool FileDoesNotExistOrEmpty(this MiruPath miruPath) =>
        miruPath.FileExists() == false || miruPath.FileInfo().Length == 0;

    public static MiruPath DeleteFileIfEmpty(this MiruPath miruPath)
    {
        if (miruPath.FileExists() && miruPath.FileInfo().Length == 0)
            miruPath.DeleteFileIfExists();

        return miruPath;
    }

    public static MiruPath EnsureDirExist(this MiruPath miruPath)
    {
        Directories.CreateIfNotExists(miruPath);
        return miruPath;
    }

    public static FileInfo FileInfo(this MiruPath miruPath) => new FileInfo(miruPath);
    
    public static bool DirContains(this MiruPath miruPath, string searchPattern) =>
        new DirectoryInfo(miruPath).GetFiles(searchPattern).Any();

    public static IEnumerable<FileInfo> Files(this MiruPath miruPath, string searchPattern) =>
        new DirectoryInfo(miruPath).GetFiles(searchPattern);

    public static bool IsDirectory(this MiruPath miruPath) => Directory.Exists(miruPath);

    public static MiruPath Dir(this MiruPath miruPath) => Path.GetDirectoryName(miruPath);
        
    public static long FileSize(this MiruPath path) => path.FileInfo().Length;
    
    public static string FileExtension(this MiruPath miruPath) => 
        Path.GetExtension(miruPath);
    
    public static string FileName(this MiruPath miruPath) => 
        Path.GetFileName(miruPath);
    
    public static string FileNameWithoutExtension(this MiruPath miruPath) => 
        Path.GetFileNameWithoutExtension(miruPath);
    
    public static bool IsFileExtension(this MiruPath miruPath, string extensionWithoutDot) => 
        Path.GetExtension(miruPath).CaseCmp($".{extensionWithoutDot}");
}