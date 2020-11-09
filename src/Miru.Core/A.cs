using System.Diagnostics;

namespace Miru.Core
{
    [DebuggerStepThrough]
    public static class A
    {
        public static MiruPath Path(string path) => new MiruPath(path);
        
        public static MiruPath Path(params string[] paths) => System.IO.Path.Combine(paths);
        
        public static MiruPath Path(string path, params string[] paths) => System.IO.Path.Combine(path, System.IO.Path.Combine(paths));
        
        public static MiruPath TempPath(params string[] paths) => System.IO.Path.Combine(
            System.IO.Path.GetTempPath(), System.IO.Path.Combine(paths));

        public static char Bar = System.IO.Path.PathSeparator;
    }
}