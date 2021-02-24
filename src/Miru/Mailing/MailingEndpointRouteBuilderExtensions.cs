using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Miru.Storages;

namespace Miru.Mailing
{
    public static class MailingEndpointRouteBuilderExtensions
    {
        public static void MapEmailsStorage(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/_emails/{index}", async context =>
            {
                var index = context.GetRouteData().Values["index"].ToInt();
            
                var emailStorage = context.RequestServices.GetService<Storage>().Temp() / "emails";

                var email = new DirectoryInfo(emailStorage)
                    .GetFiles("*.html")
                    .OrderByDescending(x => x.CreationTime)
                    .At(index);

                await context.Response.WriteAsync(await File.ReadAllTextAsync(email.FullName));
            });
                
            endpoints.MapGet("/_emails", async context =>
            {
                var emailStorage = context.RequestServices.GetService<Storage>().Temp() / "emails";

                var emails = new DirectoryInfo(emailStorage)
                    .GetFiles("*.html")
                    .OrderByDescending(x => x.CreationTime);

                var index = 0;

                await context.Response.WriteAsync($@"
<body style='font-family: Verdana'>
    <h3>Saved Emails</h3>
<ul>
    {emails.Join2(s => {
                    var link = $"<li><a href='/_emails/{index}'>{s.Name}</a></li>";
                    index++;
                    return link;
                })}
</ul>
</body>");
            });
        }
    }
}