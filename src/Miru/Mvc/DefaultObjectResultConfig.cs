namespace Miru.Mvc
{
    public class DefaultObjectResultConfig : ObjectResultConfiguration
    {
        public DefaultObjectResultConfig()
        {
            this.AddTurbolinks();
            
            this.MiruDefault();
        }
    }
}