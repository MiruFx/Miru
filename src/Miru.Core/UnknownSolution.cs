using System.Reflection;

namespace Miru.Core;

public class UnknownSolution : MiruSolution
{
    public UnknownSolution() : base(MiruPath.CurrentPath, Assembly.GetEntryAssembly().GetName().Name)
    {
        RootDir = MiruPath.CurrentPath;
        CurrentDir = MiruPath.CurrentPath;

        SrcDir = CurrentDir;
        AppDir = CurrentDir;
    }
}