using AV.Enumeration;

namespace Playground.Features.Userfy;

public class Permissions : Enumeration
{
    public static readonly Permissions ProductList = new(1, "List Products");
    public static readonly Permissions ProductEdit = new(2, "Edit Products");

    public Permissions(int value, string name) : base(value, name)
    {
    }
}

public class Permission 
{
}