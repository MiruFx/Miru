using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Miru;

public class ConfigFinder<TStartup>
{
    private readonly IEnumerable<Type> _allConfigs;

    public IEnumerable<MiruConfig> CreateInstanceOfAllConfigs() => _allConfigs
        .Where(m => m.Implements<MiruConfig>())
        .Select(type => Activator.CreateInstance(type) as MiruConfig);

    public ConfigFinder()
    {
        var assembly = typeof(TStartup).Assembly;
        var configNamespace = $"{assembly.GetName().Name}.Config";

        _allConfigs = assembly.GetTypes().Where(m => m.Namespace == configNamespace);
    }

    public object Find<TConfig>()
    {
        var configType = _allConfigs.SingleOrDefault(type => type.Implements<TConfig>());

        if (configType == null)
            return null;
            
        return Activator.CreateInstance(configType);
    }
}