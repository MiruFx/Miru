using System;
using System.Diagnostics;
using System.IO;

namespace Miru.Core
{
    [DebuggerStepThrough]
    public struct MiruPath
    {
        public static MiruPath CurrentPath => AppContext.BaseDirectory;
        
        public static readonly string DirectorySeparatorChar = Path.DirectorySeparatorChar.ToString();

        private static readonly string This = ".";
        
        private readonly string _path;

        public MiruPath(string path)
        {
            _path = path;
        }

        public static MiruPath operator /(MiruPath a, MiruPath b)
        {
            if (b.ToString().Equals(This))
                return a + DirectorySeparatorChar;
            
            return Path.Combine(a, b);
        }
        
        public static implicit operator string(MiruPath path) => path.ToString();
        
        public static implicit operator MiruPath(string path) => new MiruPath(path);

        public override string ToString() => _path;
    }
}