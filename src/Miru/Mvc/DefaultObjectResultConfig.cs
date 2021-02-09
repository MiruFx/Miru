using Miru.Turbo;

namespace Miru.Mvc
{
    public class DefaultObjectResultConfig : ObjectResultConfiguration
    {
        public DefaultObjectResultConfig()
        {
            this.MiruTurbo();
            this.MiruDefault();
        }
    }
}