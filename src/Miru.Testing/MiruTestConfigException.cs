namespace Miru.Testing
{
    public class MiruTestConfigException : MiruException
    {
        public MiruTestConfigException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}