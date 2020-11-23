using System;
using Oakton;

namespace Miru.Consolables
{
    public class MiruCommandCreator : ICommandCreator
    {
        private readonly ScopedServices _scope;

        public MiruCommandCreator(ScopedServices scope)
        {
            _scope = scope;
        }

        public IOaktonCommand CreateCommand(Type commandType)
        {
            return (IOaktonCommand)_scope.Get(commandType);
        }

        public object CreateModel(Type modelType)
        {
            return Activator.CreateInstance(modelType);
        }
    }
}