using System.Diagnostics;

namespace Miru.Core
{
    [DebuggerStepThrough]
    public static class A
    {
        public static MiruPath Root => new(string.Empty);
        
        public static MiruPath Path => new(string.Empty);
        
        public static MiruPath TempPath => new(System.IO.Path.GetTempPath());
    }
}