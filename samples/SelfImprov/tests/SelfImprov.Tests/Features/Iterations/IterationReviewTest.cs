using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miru;
using Miru.Testing;
using Miru.Testing.Userfy;
using NUnit.Framework;
using SelfImprov.Domain;
using SelfImprov.Features.Iterations;
using Shouldly;

namespace SelfImprov.Tests.Features.Iterations
{
    public class IterationReviewTest : OneCaseFeatureTest, IRequiresAuthenticatedUser
    {
        private AreasAndGoalsFixture _fix;

        public override async Task GivenAsync()
        {
            _fix = await _.ScenarioAsync<AreasAndGoalsFixture>();
        }

        [Test]
        public async Task Can_query_new_iteration_review()
        {
            var result = await _.SendAsync(new IterationReview.Query());

            // assert
            result.Areas.Count.ShouldBe(2);

            result.Areas.At(0).Name.ShouldBe(_fix.Area1.Name);

            result.Areas.At(0).Goals.Count.ShouldBe(2);
            result.Areas.At(0).Goals.At(0).Id.ShouldBe(_fix.Goal1.Id);
            result.Areas.At(0).Goals.At(1).Id.ShouldBe(_fix.Goal2.Id);

            result.Areas.At(1).Goals.Count.ShouldBe(2);
            result.Areas.At(1).Goals.At(0).Id.ShouldBe(_fix.Goal3.Id);
            result.Areas.At(1).Goals.At(1).Id.ShouldBe(_fix.Goal4.Id);
        }

        [Test]
        public async Task Should_save_iteration_review()
        {
            // act
            var command = new IterationReview.Command
            {
                Areas = new List<IterationReview.AreaReview>
                {
                    new IterationReview.AreaReview
                    {
                        Name = _fix.Area1.Name,
                        Goals = new List<IterationReview.GoalReview>
                        {
                            new IterationReview.GoalReview { Achieved = true, Name = _fix.Goal1.Name, Id = _fix.Goal1.Id },
                            new IterationReview.GoalReview { Achieved = false, Name = _fix.Goal2.Name, Id = _fix.Goal2.Id },
                        }
                    },
                    new IterationReview.AreaReview
                    {
                        Name = _fix.Area2.Name,
                        Goals = new List<IterationReview.GoalReview>
                        {
                            new IterationReview.GoalReview { Achieved = true, Name = _fix.Goal3.Name, Id = _fix.Goal3.Id },
                            new IterationReview.GoalReview { Achieved = false, Name = _fix.Goal4.Name, Id = _fix.Goal4.Id },
                        }
                    }
                }
            };

            await _.SendAsync(command);

            // assert
            var iteration = _.Db(db => db.Iterations
                .Include(i => i.Achievements)
                .ThenInclude(a => a.Goal)
                .Single());

            iteration.Number.ShouldBe(1);

            iteration.Achievements.Count.ShouldBe(4);
            iteration.Achievements.At(0).Goal.Id.ShouldBe(_fix.Goal1.Id);
            iteration.Achievements.At(1).Goal.Id.ShouldBe(_fix.Goal2.Id);
            iteration.Achievements.At(2).Goal.Id.ShouldBe(_fix.Goal3.Id);
            iteration.Achievements.At(3).Goal.Id.ShouldBe(_fix.Goal4.Id);
        }

        [Test]
        [Ignore("Use ValidationTest")]
        public void Area_should_be_required()
        {
            // var command = _.Build<IterationReview.Command>();
            // command.Validator().ShouldHaveValidationErrorFor(x => x.Areas, new List<IterationReview.Area>());
        }
    }
}
