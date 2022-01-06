using System;
using System.Linq;
using Miru.Core;

namespace Miru.Makers;

public static class MigrationMaker
{
    public static void Migration(
        this Maker m, 
        string name, 
        string version = null)
    {
        var @params = ExtractParams(name);
            
        var input = new
        {
            Name = name,
            Version = version ?? DateTime.Now.ToString("yyyyMMddHHmm"),
            Table = @params.TableName,
            Column = @params.ColumnName,
        };

        var templateName = name.StartsWith("Alter") ? "MigrationAlter" : "Migration";
            
        m.Template(templateName, input, m.Solution.MigrationsDir / $"{input.Version}_{name}.cs");
    }

    private static Params ExtractParams(string name)
    {
        if (name.StartsWith("Alter"))
        {
            var inputs = name.GetStringInBetween("Alter", "Add");
            return new Params
            {
                TableName = inputs.ElementAtOrDefault(0),
                ColumnName = inputs.ElementAtOrDefault(1)
            };
        }

        if (name.StartsWith("Create"))
        {
            return new Params
            {
                TableName = name.Replace("Create", string.Empty)
            };
        }

        return new Params();
    }

    public class Params
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
    }
    
    // https://extensionmethod.net/csharp/stringsplitoptions/getstringinbetween
    public static string[] GetStringInBetween(
        this string strSource, 
        string strBegin, 
        string strEnd, 
        bool includeBegin = false, 
        bool includeEnd = false)
    {
        string[] result = { "", "" };

        int iIndexOfBegin = strSource.IndexOf(strBegin, StringComparison.Ordinal);

        if (iIndexOfBegin != -1)
        {
            // include the Begin string if desired
            if (includeBegin)
                iIndexOfBegin -= strBegin.Length;

            strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);

            int iEnd = strSource.IndexOf(strEnd, StringComparison.Ordinal);

            if (iEnd != -1)
            {
                // include the End string if desired
                if (includeEnd)
                    iEnd += strEnd.Length;

                result[0] = strSource.Substring(0, iEnd);

                // advance beyond this segment
                if (iEnd + strEnd.Length < strSource.Length)
                    result[1] = strSource.Substring(iEnd + strEnd.Length);
            }
        }
        else
            // stay where we are
            result[1] = strSource;

        return result;
    }
}
