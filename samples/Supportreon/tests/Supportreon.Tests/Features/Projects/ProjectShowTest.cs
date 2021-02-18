using System.Threading.Tasks;
using Miru;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Supportreon.Domain;
using Supportreon.Features.Projects;

namespace Supportreon.Tests.Features.Projects
{
    public class ProjectShowTest : OneCaseFeatureTest
    {
        [Test]
        public async Task Can_show_project()
        {
            // arrange
            var users = _.MakeMany<User>();
            var project = _.Make<Project>();
            var donations = _.MakeMany<Donation>(10, m =>
            {
                m.Project = project;
                m.User = _.Faker().PickRandom(users);
            });

            await _.SaveAsync(users, project, donations);
            
            // act
            var result = await _.SendAsync(new ProjectShow.Query { Id = project.Id });
            
            // assert
            result.Project.ShouldBe(project);
            result.Project.User.ShouldNotBeNull();
            
            result.LastDonations.At(0).ShouldBe(donations.At(9));
            result.LastDonations.At(4).ShouldBe(donations.At(5));
            
            result.LastDonations.At(0).User.Name.ShouldBe(donations.At(9).User.Name);
        }
    }
}
