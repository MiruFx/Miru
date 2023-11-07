namespace Miru.Fabrication;

public class FabricatedSession
{
    private readonly Dictionary<Type, object> _singletons = new();
    private readonly List<object> _fabricated = new();
        
    public object GetSingleton(Type type)
    {
        if (_singletons.TryGetValue(type, out object instance))
            return instance;
            
        return null;
    }

    public void AddSingleton(Type type, object instance)
    {
        _singletons.Add(type, instance);
    }
        
    public void Add(object fabricated)
    {
        _fabricated.Add(fabricated);
    }
        
    public void AddMany(IEnumerable<object> fabricated)
    {
        fabricated.Each(m => _fabricated.Add(m));
    }

    public IReadOnlyList<object> GetAllFabricated()
    {
        return _fabricated;
    }

    public void Clear()
    {
        _fabricated.Clear();
        _singletons.Clear();
    }

    public override string ToString()
    {
        return $"{base.ToString()}@{base.GetHashCode()}";
    }
}