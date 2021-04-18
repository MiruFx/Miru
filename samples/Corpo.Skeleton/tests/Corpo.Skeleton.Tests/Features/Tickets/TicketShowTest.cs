using System.Threading.Tasks;
using Corpo.Skeleton.Features.Tickets;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.Tests.Features.Tickets
{
    public class TicketShowTest : FeatureTest
    {
        [Test]
        public async Task Can_show_ticket()
        {
            // arrange
            // var ticket = _.MakeSaving<Ticket>();
            
            // act
            var result = await _.SendAsync(new TicketShow.Query { Id = 1 });
            
            // assert
            // result.ShouldBe();
        }
    }
}