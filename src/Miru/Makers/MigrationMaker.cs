using System;
using Miru.Core;

namespace Miru.Makers
{
    public static class MigrationMaker
    {
        public static void Migration(
            this Maker m, 
            string name, 
            string version = null, 
            string table = null)
        {
            var input = new
            {
                Name = name,
                Version = version ?? DateTime.Now.ToString("yyyyMMddHHmm"),
                Table = table ?? "TableName"
            };

            var templateName = "Migration";

            if (name.StartsWith("Alter"))
                templateName = "MigrationAlter";
            
            m.Template(templateName, input, m.Solution.MigrationsDir / $"{input.Version}_{name}.cs");
        }
    }
}