using System;
using System.Diagnostics;
using System.IO;

namespace Miru.Core;

[DebuggerStepThrough]
public struct MiruPath
{
    public static MiruPath CurrentPath => AppContext.BaseDirectory;
        
    private static readonly string This = ".";
        
    private string PathString { get; }
    private bool AltSeparator { get; }

    public MiruPath(string path, bool altSeparator = false)
    {
        PathString = path;
        AltSeparator = altSeparator;
    }

    public static MiruPath operator /(MiruPath a, MiruPath b)
    {
        if (b.PathString.Equals(This))
            return a + Path.DirectorySeparatorChar;
            
        return Path.Combine(a, b);
    }

    public static MiruPath operator /(MiruPath a, object b)
    {
        if (b is string @string)
            return a / @string;
        
        return a / b.ToString();   
    }
        
    public static implicit operator string(MiruPath path) => path.PathString;
        
    public static implicit operator MiruPath(string path) => new(path);

    public override string ToString() => AltSeparator == false 
        ? PathString 
        : PathString.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
}