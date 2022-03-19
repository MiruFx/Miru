using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Tickets;
using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.PageTests.Pages.Tickets;

public class TicketEditPageTest : PageTest
{
    [Test]
    public void Can_edit_ticket()
    {
        // arrange
        var ticket = _.MakeSaving<Ticket>();
            
        _.Visit(new TicketEdit.Query { Id = ticket.Id });

        // act
        _.Form<TicketEdit.Command>((f, command) =>
        {
            f.Input(m => m.Name, command.Name);
                
            f.Submit();
        });
            
        // assert
        _.ShouldHaveText("Ticket successfully saved");
    }
}