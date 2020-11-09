using System.Reflection;

namespace Miru.Core
{
    public class MiruInfo
    {
        public static string MiruVersion
        {
            get
            {
                var v = typeof(MiruInfo).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

                var plusPos = v.IndexOf('+');
                
                return plusPos > 0 ? v.Substring(0, plusPos) : v;
            }    
        }
    }
}