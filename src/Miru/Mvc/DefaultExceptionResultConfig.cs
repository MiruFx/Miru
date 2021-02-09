using Miru.Turbo;

namespace Miru.Mvc
{
    public class DefaultExceptionResultConfig : ExceptionResultConfiguration
    {
        public DefaultExceptionResultConfig()
        {
            this.MiruTurbo();
            this.MiruDefault();
        }
    }
}