using System.IO;

namespace Miru.Storages;

public static class MiruPathExtensions
{
    public static MiruPath RelativeTo(this MiruPath fullPath, IStorage storage) =>
        storage.RelativePath(fullPath);
    
    public static MiruPath FullPathTo(this MiruPath relativePath, IStorage storage) =>
        storage.Path / relativePath;

    public static MiruPath AltSeparator(this MiruPath path) =>
        path.ToString().Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

    /// <summary>
    /// Returns the new renamed path
    /// </summary>
    public static MiruPath Rename(this MiruPath path, string newNameWithExtension)
    {
        var newPath = path.Dir() / newNameWithExtension;
        
        File.Move(path, newPath);

        return newPath;
    }
    
    /// <summary>
    /// Returns the new renamed path
    /// </summary>
    public static MiruPath AddExtension(this MiruPath path, string extensionWithoutDot)
    {
        var newPath = $"{path}.{extensionWithoutDot}";
        
        File.Move(path, newPath);

        return newPath;
    }
    
    public static void CopyTo(this MiruPath path, MiruPath destination)
    {
        destination.Dir().EnsureDirExist();
        
        File.Copy(path, destination);
    }

    public static MiruPath RandomFileName(this MiruPath path, string extension) => 
        path / $"{Path.GetRandomFileName()}.{extension}";
}