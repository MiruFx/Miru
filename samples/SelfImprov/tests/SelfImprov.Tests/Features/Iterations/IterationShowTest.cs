using System.Collections.Generic;
using System.Threading.Tasks;
using Miru;
using Miru.Testing;
using Miru.Testing.Userfy;
using NUnit.Framework;
using SelfImprov.Domain;
using SelfImprov.Features.Iterations;
using Shouldly;

namespace SelfImprov.Tests.Features.Iterations
{
    public class IterationShowTest : OneCaseFeatureTest, IRequiresAuthenticatedUser
    {
        private AreasAndGoalsFixture _fix;
        private Iteration _iteration;
        private IterationShow.Result _result;

        public override async Task Given()
        {
            // TODO: Change this syntax. I can't compose scenarios
            // Would be nice to compose with another scenario that sets Achievements
            
            _fix = await _.Scenario<AreasAndGoalsFixture>();
            
            var ach1 = _.Make<Achievement>(x => x.GoalId = _fix.Goal1.Id, x => x.Achieved = true);
            var ach2 = _.Make<Achievement>(x => x.GoalId = _fix.Goal2.Id, x => x.Achieved = true);
            var ach3 = _.Make<Achievement>(x => x.GoalId = _fix.Goal3.Id, x => x.Achieved = true);
            var ach4 = _.Make<Achievement>(x => x.GoalId = _fix.Goal4.Id, x => x.Achieved = false);
            var achDeletedArea = _.Make<Achievement>(x => x.GoalId = _fix.DeletedGoal.Id, x => x.Achieved = true);
            var achDeletedArea1 = _.Make<Achievement>(x => x.GoalId = _fix.DeletedGoalArea1.Id, x => x.Achieved = false);

            _iteration = _.Make<Iteration>(x => x.Achievements = new List<Achievement>
            {
                ach1, ach2, ach3, ach4, achDeletedArea, achDeletedArea1
            });

            await _.Save(_iteration, ach1, ach2, ach3, ach4, achDeletedArea, achDeletedArea1);
            
            // act
            _result = await _.Send(new IterationShow.Query { Id = _iteration.Id });
        }

        [Test]
        public void Should_show_percent_achieved_per_area()
        {
            var areas = _result.Iteration.GetAchievementsPerArea();
            areas.At(0).PercentAchieved.ShouldBeAprox(66.66m);
            areas.At(1).PercentAchieved.ShouldBe(50);
            areas.At(2).PercentAchieved.ShouldBe(100);
        }

        [Test]
        public void Should_show_iteration_percent_achieved()
        {
            _iteration.PercentAchieved.ShouldBeAprox(66.66m);
        }

        [Test]
        public void Should_group_by_area()
        {
            var areas = _result.Iteration.GetAchievementsPerArea();
            
            areas.ShouldCount(3);
            
            areas.At(0).Area.Name.ShouldBe(_fix.Area1.Name);
            areas.At(1).Area.Name.ShouldBe(_fix.Area2.Name);
            areas.At(2).Area.Name.ShouldBe(_fix.DeletedArea.Name);
        }

        [Test]
        public void Should_show_achievements_per_area()
        {
            var areas = _result.Iteration.GetAchievementsPerArea();
            var achievementsArea1 = areas.At(0);
            var achievementsArea2 = areas.At(1);

            // area 1
            achievementsArea1.Achievements.At(0).Goal.Name.ShouldBe(_fix.Goal1.Name);
            achievementsArea1.Achievements.At(0).Achieved.ShouldBeTrue();

            achievementsArea1.Achievements.At(1).Goal.Name.ShouldBe(_fix.Goal2.Name);
            achievementsArea1.Achievements.At(1).Achieved.ShouldBeTrue();

            // area 2
            achievementsArea2.Achievements.At(0).Goal.Name.ShouldBe(_fix.Goal3.Name);
            achievementsArea2.Achievements.At(0).Achieved.ShouldBeTrue();

            achievementsArea2.Achievements.At(1).Goal.Name.ShouldBe(_fix.Goal4.Name);
            achievementsArea2.Achievements.At(1).Achieved.ShouldBeFalse();
        }

        //[Test]
        //public void Should_show_iteration()
        //{
        //result.Id.ShouldBe(iteration.Id);
        //result.Number.ShouldBe(iteration.Number);
        //result.PercentAchieved.ShouldBe(75);

        //result.Achievements.Count.ShouldBe(4);
        //result.Achievements.At(0).Goal.Area.Name.ShouldBe(area1.Name);

        //result.Areas.At(0).Achievements.At(0).GoalName.ShouldBe(goal1.Name);
        //result.Areas.At(0).Achievements.At(0).Achieved.ShouldBe(achievement1.Achieved);
        //result.Areas.At(0).Achievements.At(1).GoalName.ShouldBe(goal2.Name);
        //result.Areas.At(0).Achievements.At(1).Achieved.ShouldBe(achievement2.Achieved);

        //result.Areas.At(1).Name.ShouldBe(area2.Name);
        //result.Areas.At(1).Achievements.At(0).GoalName.ShouldBe(goal3.Name);
        //result.Areas.At(1).Achievements.At(0).Achieved.ShouldBe(achievement3.Achieved);
        //result.Areas.At(1).Achievements.At(1).GoalName.ShouldBe(goal4.Name);
        //result.Areas.At(1).Achievements.At(1).Achieved.ShouldBe(achievement4.Achieved);

        //result.Achievements.Count().ShouldBe(4);
        //result.Achievements.At(0).GoalAreaName.ShouldBe(area1.Name);
        //result.Achievements.At(0).GoalName.ShouldBe(goal1.Name);
        //result.Achievements.At(0).Achieved.ShouldBe(achievement1.Achieved);

        //result.Achievements.At(1).GoalAreaName.ShouldBe(area1.Name);
        //result.Achievements.At(1).GoalName.ShouldBe(goal2.Name);
        //result.Achievements.At(1).Achieved.ShouldBe(achievement2.Achieved);

        //result.Achievements.At(2).GoalAreaName.ShouldBe(area2.Name);
        //result.Achievements.At(2).GoalName.ShouldBe(goal3.Name);
        //result.Achievements.At(2).Achieved.ShouldBe(achievement3.Achieved);

        //result.Achievements.At(3).GoalAreaName.ShouldBe(area2.Name);
        //result.Achievements.At(3).GoalName.ShouldBe(goal4.Name);
        //result.Achievements.At(3).Achieved.ShouldBe(achievement4.Achieved);
        //}


        //[Test]
        //public void Should_show_iteration()
        //{
        //    // TODO: try Bogus to generate scenario

        //    var area1 = Fixture.Build<Area>(x => x.Name = "Area1");
        //    var area2 = Fixture.Build<Area>(x => x.Name = "Area2");
        //    var goal1 = Fixture.Build<Goal>(x => x.Area = area1, x => x.Name = "Goal1");
        //    var goal2 = Fixture.Build<Goal>(x => x.Area = area1, x => x.Name = "Goal2");
        //    var goal3 = Fixture.Build<Goal>(x => x.Area = area2, x => x.Name = "Goal3");
        //    var goal4 = Fixture.Build<Goal>(x => x.Area = area2, x => x.Name = "Goal4");
        //    var achievement1 = Fixture.Build<Achievement>(x => x.Goal = goal1);
        //    var achievement2 = Fixture.Build<Achievement>(x => x.Goal = goal2);
        //    var achievement3 = Fixture.Build<Achievement>(x => x.Goal = goal3);
        //    var achievement4 = Fixture.Build<Achievement>(x => x.Goal = goal4);
        //    var iteration = Fixture.Build<Iteration>();

        //    iteration.Achievements.Clear();
        //    iteration.Achievements.Add(achievement1);
        //    iteration.Achievements.Add(achievement2);

        //    Fixture.Persist(iteration, achievement1, achievement2, achievement3, achievement4);

        //    var result = Send(new IterationShow.Query { Id = iteration.Id });

        //    result.Id.ShouldBe(iteration.Id);
        //    result.Number.ShouldBe(iteration.Number);
        //    result.PercentAchieved.ShouldBe(50);
        //    result.CreatedAt.ShouldBe(iteration.CreatedAt, TimeSpan.FromSeconds(0.1));

        //    result.Areas.Count().ShouldBe(2);
        //    result.Areas.At(0).Name.ShouldBe(area1.Name);

        //    result.Areas.At(0).Achievements.At(0).GoalName.ShouldBe(goal1.Name);
        //    result.Areas.At(0).Achievements.At(0).Achieved.ShouldBe(achievement1.Achieved);
        //    result.Areas.At(0).Achievements.At(1).GoalName.ShouldBe(goal2.Name);
        //    result.Areas.At(0).Achievements.At(1).Achieved.ShouldBe(achievement2.Achieved);

        //    result.Areas.At(1).Name.ShouldBe(area2.Name);
        //    result.Areas.At(1).Achievements.At(0).GoalName.ShouldBe(goal3.Name);
        //    result.Areas.At(1).Achievements.At(0).Achieved.ShouldBe(achievement3.Achieved);
        //    result.Areas.At(1).Achievements.At(1).GoalName.ShouldBe(goal4.Name);
        //    result.Areas.At(1).Achievements.At(1).Achieved.ShouldBe(achievement4.Achieved);

        //    //result.Achievements.Count().ShouldBe(4);
        //    //result.Achievements.At(0).GoalAreaName.ShouldBe(area1.Name);
        //    //result.Achievements.At(0).GoalName.ShouldBe(goal1.Name);
        //    //result.Achievements.At(0).Achieved.ShouldBe(achievement1.Achieved);

        //    //result.Achievements.At(1).GoalAreaName.ShouldBe(area1.Name);
        //    //result.Achievements.At(1).GoalName.ShouldBe(goal2.Name);
        //    //result.Achievements.At(1).Achieved.ShouldBe(achievement2.Achieved);

        //    //result.Achievements.At(2).GoalAreaName.ShouldBe(area2.Name);
        //    //result.Achievements.At(2).GoalName.ShouldBe(goal3.Name);
        //    //result.Achievements.At(2).Achieved.ShouldBe(achievement3.Achieved);

        //    //result.Achievements.At(3).GoalAreaName.ShouldBe(area2.Name);
        //    //result.Achievements.At(3).GoalName.ShouldBe(goal4.Name);
        //    //result.Achievements.At(3).Achieved.ShouldBe(achievement4.Achieved);
        //}
    }
}
