using System.Collections.Generic;

namespace Miru.Databases;

public class DatabaseCleanerOptions
{
    private readonly List<string> _tablesToIgnore = new List<string>();

    public void AddTableToIgnore(string table) => _tablesToIgnore.Add(table);

    public IEnumerable<string> TablesToIgnore => _tablesToIgnore;
}