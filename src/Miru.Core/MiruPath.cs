using System;
using System.Diagnostics;
using System.IO;

namespace Miru.Core;

[DebuggerStepThrough]
public readonly struct MiruPath(string path)
{
    public static MiruPath CurrentPath => AppContext.BaseDirectory;
        
    public static readonly string DirectorySeparatorChar = Path.DirectorySeparatorChar.ToString();

    private static readonly string This = ".";

    public static MiruPath operator /(MiruPath a, MiruPath b)
    {
        if (b.ToString().Equals(This))
            return a + DirectorySeparatorChar;
            
        return Path.Combine(a, b);
    }

    public static MiruPath operator /(MiruPath a, object b) => a / b.ToString();
        
    public static implicit operator string(MiruPath path) => path.ToString();
        
    public static implicit operator MiruPath(string path) => new(path);

    public override string ToString() => path;
}