using Miru.Core;

namespace Miru.Makers
{
    public static class EntityMaker
    {
        public static void Entity(this Maker m, string name)
        {
            var input = new
            {
                Name = name
            };
            
            m.Template("Entity", input, m.Solution.DomainDir / $"{name}.cs");
            
            m.Template("EntityTest", input, m.Solution.AppTestsDir / "Domain" / $"{name}Test.cs");
        }
    }
}