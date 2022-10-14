using Ardalis.SmartEnum;

namespace Playground.Features.Userfy;

public class Permissions : SmartEnum<Permissions>
{
    public static readonly Permissions ProductList = new(1, "List Products");
    public static readonly Permissions ProductEdit = new(2, "Edit Products");

    public Permissions(int value, string name) : base(name, value)
    {
    }
}

public class Permission 
{
}