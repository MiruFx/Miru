using Corpo.Skeleton.Features.Tickets;

namespace Corpo.Skeleton.Tests.Features.Tickets;

public class TicketDoneTest : FeatureTest
{
    [Test]
    public async Task Can_list_teams()
    {
        // arrange
        // var tickets = _.MakeManySaving<Ticket>();
            
        // act
        var result = await _.SendAsync(new TicketDone.Query());
            
        // assert
        result.Tickets.ShouldNotBeNull();
        
        // result.Tickets.First().Name.ShouldBe(tickets.First().Name);
        // result.Tickets.Last().Name.ShouldBe(tickets.Last().Name);
    }
}