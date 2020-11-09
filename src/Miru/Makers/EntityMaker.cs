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
            
            m.Template("Entity", input, A.Path(m.Solution.DomainDir, $"{name}.cs"));
            
            m.Template("EntityTest", input, A.Path(m.Solution.AppTestsDir, "Domain", $"{name}Test.cs"));
        }
    }
}