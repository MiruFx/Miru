using System.Threading.Tasks;
using Miru.Testing;
using SelfImprov.Domain;

namespace SelfImprov.Tests.Features
{
    public class AreasAndGoalsFixture : IFixtureScenario
    {
        public Area Area1 { get; set; }
        public Area Area2 { get; set; }
        public Goal Goal1 { get; set; }
        public Goal Goal2 { get; set; }
        public Goal Goal3 { get; set; }
        public Goal Goal4 { get; set; }
        
        public Area DeletedArea { get; set; }
        public Goal DeletedGoal { get; set; }
        public Goal DeletedGoalArea1 { get; set; }
        
        public async Task Build(ITestFixture _)
        {
            Area1 = _.Make<Area>();
            Area2 = _.Make<Area>();
            Goal1 = _.Make<Goal>(x => x.Area = Area1);
            Goal2 = _.Make<Goal>(x => x.Area = Area1);
            Goal3 = _.Make<Goal>(x => x.Area = Area2);
            Goal4 = _.Make<Goal>(x => x.Area = Area2);

            DeletedArea = _.Make<Area>(m => m.IsInactive = true);
            DeletedGoal = _.Make<Goal>(m => { m.Area = DeletedArea; m.IsInactive = true; });
            DeletedGoalArea1 = _.Make<Goal>(m => { m.Area = Area1; m.IsInactive = true; });

            // TODO: _.SaveMade()
            await _.Save(Area1, Area2, Goal1, Goal2, Goal3, Goal4, DeletedArea, DeletedGoal, DeletedGoalArea1);
        }
    }
}