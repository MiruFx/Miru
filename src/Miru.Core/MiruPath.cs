using System;
using System.Diagnostics;
using System.IO;

namespace Miru.Core;

[DebuggerStepThrough]
public readonly struct MiruPath
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
        if (a.PathString.Length == 0)
            return b;
        
        if (a.PathString[0] == Path.DirectorySeparatorChar || a.PathString[0] == Path.AltDirectorySeparatorChar)
            return b;
            
        if (b.PathString.Equals(This))
            return a;

        if (a.PathString[^1] == a.GetSeparatorChar)
            return a + b;
        
        return new MiruPath(a + a.GetSeparatorChar + b, a.AltSeparator);
    }

    public static MiruPath operator /(MiruPath a, object b)
    {
        if (b is string @string)
            return a / @string;

        return a / b.ToString();
    }
        
    public static implicit operator string(MiruPath path) => path.PathString;
        
    public static implicit operator MiruPath(string path) => new(path);

    public override string ToString() => PathString;

    private char GetSeparatorChar => AltSeparator == false 
        ? Path.DirectorySeparatorChar 
        : Path.AltDirectorySeparatorChar;
}