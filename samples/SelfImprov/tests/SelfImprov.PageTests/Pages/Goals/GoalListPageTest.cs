using System.Threading.Tasks;
using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;
using OpenQA.Selenium;
using SelfImprov.Features.Goals;

namespace SelfImprov.PageTests.Pages.Goals
{
    public class GoalListPageTest : PageTest
    {
        private GoalsFixture _fix;

        public override async Task Given()
        {
            _fix = await _.Scenario<GoalsFixture>();
            
            _.Visit<GoalList>();
        }

        [Test]
        public void Can_list_goals()
        {
            _.ShouldHaveText(_fix.Work.Name);
            _.ShouldHaveText(_fix.Health.Name);
            _.ShouldHaveText(_fix.WorkLess.Name);
            _.ShouldHaveText(_fix.DrinkWater.Name);
        }

        [Test]
        public void Can_add_new_area()
        {
            _.ClickLink("New Area");

            _.Form<AreaNew.Command>(f =>
            {
                f.Input(m => m.Name, "Italian");
                f.Submit();
            });

            _.Within(By.Id("areas"), areas =>
            {
                areas.ShouldHaveText("Italian");
            });
        }

        [Test]
        public void Can_add_new_goal_for_an_area()
        {
            _.Within(By.Id($"area_{_fix.Health.Id}"), area =>
            {
                area.ClickLink("Add a Goal");
                
                area.FormFor<GoalNew.Command>($"goal-new-area_{_fix.Health.Id}", f =>
                {
                    f.Input2(m => m.Name, "Run 3 times per week");
                    f.Submit();
                });
            });
            
            _.ShouldHaveText("Run 3 times per week");
        }
        
        [Test]
        public void Can_edit_a_goal()
        {
            _.Within(By.Id($"goal-{_fix.GoalEdit.Id}"), w =>
            {
                w.ClickLink("Edit");
                
                w.FormFor<GoalEdit.Command>($"goal-edit_{_fix.GoalEdit.Id}", f =>
                {
                    f.Input(m => m.Name, "New Goal Name");
                    f.Submit();
                });
            });
            
            _.ShouldHaveText("New Goal Name");
        }
        
        [Test]
        public void Can_remove_a_goal()
        {
            _.Within(By.Id($"goal-{_fix.GoalRemove.Id}"), w =>
            {
                w.ClickLink("Remove");
            });
            
            _.ShouldNotHaveText(_fix.GoalRemove.Name);
        }
        
        [Test]
        public void Can_remove_a_area()
        {
            _.Within(By.Id($"area-show_{_fix.AreaToRemove.Id}"), w =>
            {
                // TODO: ClickFor<AreaRemove>()  .. data-feature='area-remove'
                w.Click(By.Id("area-remove-link"));
            });
            
            _.ShouldNotHaveText(_fix.AreaToRemove.Name);
            _.ShouldNotHaveText(_fix.GoalAreaToRemove.Name);
        }
        
        [Test]
        public void Can_edit_area()
        {
            // TODO: _.Within(Area)
            _.Within(By.Id($"area_{_fix.AreaToEdit.Id}"), w =>
            {
                // TODO: ClickFor<AreaEdit>()  .. data-feature='area-remove'
                w.Click(By.Id("area-edit-link"));
                
                w.Form<AreaEdit.Command>(f =>
                {
                    f.Input(m => m.Name, "New Area Name");
                    f.Submit();
                });
            });
            
            _.ShouldHaveText("New Area Name");
        }

        [Test]
        public void Cannot_leave_new_area_name_blank()
        {
            _.ClickLink("New Area");

            _.Form<AreaNew.Command>(f =>
            {
                f.Input(m => m.Name, string.Empty);
                f.Submit();

                f.ShouldHaveText("'Name' must not be empty");
                f.ClickLink("Cancel");
            });
        }
        
        [Test]
        public void Cannot_leave_area_name_blank()
        {
            // TODO: _.Within(Area)
            _.Within(By.Id($"area_{_fix.AreaToEdit.Id}"), w =>
            {
                // TODO: ClickFor<AreaEdit>()  .. data-feature='area-remove'
                w.Click(By.Id("area-edit-link"));
                
                w.Form<AreaEdit.Command>(f =>
                {
                    f.Input(m => m.Name, string.Empty);
                    f.Submit();
                    
                    f.ShouldHaveText("'Name' must not be empty");
                    f.ClickLink("Cancel");
                });
            });
        }

        [Test]
        public void Cannot_leave_new_goal_name_blank()
        {
            _.Within(By.Id($"area_{_fix.Health.Id}"), area =>
            {
                area.ClickLink("Add a Goal");
                
                area.FormFor<GoalNew.Command>($"goal-new-area_{_fix.Health.Id}", f =>
                {
                    f.Input2(m => m.Name, string.Empty);
                    f.Submit();
                });
            });
            
            _.ShouldHaveText("'Name' must not be empty");
        }

        [Test]
        public void Cannot_leave_goal_name_blank()
        {
            _.Within(By.Id($"goal-{_fix.GoalEdit.Id}"), w =>
            {
                w.ClickLink("Edit");
                
                w.FormFor<GoalEdit.Command>($"goal-edit_{_fix.GoalEdit.Id}", f =>
                {
                    f.Input(m => m.Name, string.Empty);
                    f.Submit();
                });
            });
            
            _.ShouldHaveText("'Name' must not be empty");
        }
    }
}