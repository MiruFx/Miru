using Miru.Userfy;
using Vereyon.Web;

namespace Miru.Mvc
{
    public static class ExceptionResultContextExtension
    {
        public static IFlashMessage Flash(this ExceptionResultContext ctx) => 
            ctx.GetService<IFlashMessage>();        
        
        public static UserfyOptions UserfyOptions(this ExceptionResultContext ctx) => 
            ctx.GetService<UserfyOptions>();
    }
}