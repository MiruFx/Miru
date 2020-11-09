using System.Threading.Tasks;
using Miru.Testing;
using SelfImprov.Domain;

namespace SelfImprov.PageTests.Pages.Goals
{
    public class GoalsFixture : IFixtureScenario
    {
        public User User { get; set; }
        public Area Work { get; set; }
        public Area Health { get; set; }
        public Area AreaToRemove { get; set; }
        public Goal WorkLess { get; set; }
        public Goal DrinkWater { get; set; }
        public Goal GoalEdit { get; set; }
        public Goal GoalRemove { get; set; }
        public Goal GoalAreaToRemove { get; set; }
        public Area AreaToEdit { get; set; }
        
        public async Task Build(ITestFixture _)
        {
            User = _.MakeSavingLogin<User>();
            
            Work = _.Make<Area>();
            WorkLess = _.Make<Goal>(m => m.Area = Work);

            Health = _.Make<Area>();
            DrinkWater = _.Make<Goal>(m => m.Area = Health);
            GoalEdit = _.Make<Goal>(m => m.Area = Health);
            GoalRemove = _.Make<Goal>(m => m.Area = Health);
            
            AreaToRemove = _.Make<Area>();
            GoalAreaToRemove = _.Make<Goal>(m => m.Area = AreaToRemove);
            
            AreaToEdit = _.Make<Area>();
            
            await _.Save(
                Work, WorkLess, 
                Health, DrinkWater, GoalEdit, GoalRemove,
                AreaToRemove, GoalAreaToRemove,
                AreaToEdit);
        }
    }
}