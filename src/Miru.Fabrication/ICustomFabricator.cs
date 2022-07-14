using System.Collections.Generic;

namespace Miru.Fabrication;

public interface ICustomFabricator
{
}
    
public interface ICustomFabricator<out T> : ICustomFabricator where T : class
{
    T Make();
        
    IEnumerable<T> MakeMany(int count);
}