using System.Threading.Tasks;
using Corpo.Skeleton.Features.Teams;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.Tests.Features.Tickets
{
    public class TicketEditTest : FeatureTest
    {
        [Test]
        public async Task Can_edit_ticket()
        {
            // arrange
            // var ticket = _.MakeSaving<Ticket>();

            // act
            // var result = await _.SendAsync(new TicketEdit.Command
            // {
            //     Id = ticket.Id
            // });

            // assert
            // var saved = _.Db(db => db.Tickets.First());
            // saved.Name.ShouldBe(command.Name);
        }

        public class Validations : ValidationTest<TeamEdit.Command>
        {
            [Test]
            public void Name_is_required()
            {
                // ShouldBeValid(m => m.Name, Request.Name);
                //
                // ShouldBeInvalid(m => m.Name, string.Empty);
            }
        }
    }
}