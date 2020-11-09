using System;
using Miru.Core;

namespace Miru.Makers
{
    public static class MigrationMaker
    {
        public static void Migration(this Maker m, string name, string version = null)
        {
            var input = new
            {
                Name = name,
                Version = version ?? DateTime.Now.ToString("yyyyMMddHHmm")
            };
            
            m.Template("Migration", input, A.Path(m.Solution.MigrationsDir, $"{input.Version}_{name}.cs"));
        }
    }
}