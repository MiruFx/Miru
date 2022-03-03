using Corpo.Skeleton.Features.Tickets;

namespace Corpo.Skeleton.Tests.Features.Tickets;

public class TicketEditTest : FeatureTest
{
    [Test]
    public async Task Can_edit_ticket()
    {
        // arrange
        // var ticket = _.MakeSaving<Ticket>();
        var command = _.Make<TicketEdit.Command>();

        // act
        var result = await _.SendAsync(command);

        // assert
        // var saved = _.Db(db => db.Tickets.First());
        // saved.Name.ShouldBe(command.Name);
    }

    public class Validations : ValidationTest<TicketEdit.Command>
    {
        [Test]
        public void Name_is_required()
        {
            ShouldBeValid(m => m.Name, Request.Name);
            
            ShouldBeInvalid(m => m.Name, string.Empty);
        }
    }
}