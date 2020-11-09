using System;
using Oakton;

namespace Miru.Consolables
{
    public class MiruCommandCreator : ICommandCreator
    {
        private readonly IMiruApp _app;

        public MiruCommandCreator(IMiruApp app)
        {
            _app = app;
        }

        public IOaktonCommand CreateCommand(Type commandType)
        {
            return (IOaktonCommand)_app.Get(commandType);
        }

        public object CreateModel(Type modelType)
        {
            return Activator.CreateInstance(modelType);
        }
    }
}