using System;
using System.Collections.Generic;

namespace Miru.Testing;

public class TestSetupActions
{
    private readonly Dictionary<Type, List<Action<Type, TestFixture>>> _items = new();
        
    public void Add(Type type, Action<Type, TestFixture> action)
    {
        if (!_items.TryGetValue(type, out var listForType))
        {
            listForType = new List<Action<Type, TestFixture>>();
            _items[type] = listForType;
        }
            
        listForType.Add(action);
    }
        
    public IEnumerable<KeyValuePair<Type, Action<Type, TestFixture>>> All()
    {
        foreach (var item in _items)
        {
            var actions = item.Value;
                
            if (actions == null)
                continue;
                
            foreach (var action in actions)
            {
                yield return new KeyValuePair<Type, Action<Type, TestFixture>>(item.Key, action);
            }
        }
    }
}