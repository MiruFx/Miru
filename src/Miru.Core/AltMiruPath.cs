using System;
using System.Diagnostics;
using System.IO;

namespace Miru.Core;

[DebuggerStepThrough]
internal readonly struct AltMiruPath
{
    public static MiruPath CurrentPath => AppContext.BaseDirectory;
        
    private static readonly string This = ".";
        
    private string PathString { get; }
    private bool AltSeparator { get; }

    public AltMiruPath(string path, bool altSeparator = false)
    {
        PathString = path;
        AltSeparator = altSeparator;
    }

    public static AltMiruPath operator /(AltMiruPath a, AltMiruPath b)
    {
        if (a.PathString.Length == 0)
            return b;
        
        if (a.PathString[0] == Path.DirectorySeparatorChar || a.PathString[0] == Path.AltDirectorySeparatorChar)
            return b;
            
        if (b.PathString.Equals(This))
            return a;

        if (a.PathString[^1] == a.GetSeparatorChar)
            return a + b;
        
        return new AltMiruPath(a + a.GetSeparatorChar + b, a.AltSeparator);
    }

    public static AltMiruPath operator /(AltMiruPath a, object b)
    {
        if (b is string @string)
            return a / @string;

        return a / b.ToString();
    }
        
    public static implicit operator string(AltMiruPath path) => path.PathString;
        
    public static implicit operator AltMiruPath(string path) => new(path);

    public override string ToString() => PathString;

    private char GetSeparatorChar => AltSeparator == false 
        ? Path.DirectorySeparatorChar 
        : Path.AltDirectorySeparatorChar;
}