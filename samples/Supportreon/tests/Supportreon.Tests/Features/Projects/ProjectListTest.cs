using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Miru;
using Miru.Testing;
using NUnit.Framework;
using Supportreon.Domain;
using Supportreon.Features.Projects;

namespace Supportreon.Tests.Features.Projects
{
    public class ProjectListTest : OneCaseFeatureTest
    {
        private IEnumerable<Project> _artProjects;
        private IEnumerable<Project> _foodProjects;
        private Project _closedProject;
        private Category _art;
        private Category _food;

        public override async Task GivenAsync()
        {
            _art = _.Make<Category>();
            _food = _.Make<Category>();
            
            _artProjects = _.MakeMany<Project>(2, m => m.Category = _art);
            _foodProjects = _.MakeMany<Project>(3, m => m.Category = _food);

            _closedProject = _.Make<Project>(m => m.EndDate = DateTime.Now);
            
            await _.SaveAsync(_art, _food, _artProjects, _foodProjects, _closedProject);
        }

        [Test]
        public async Task Can_list_projects()
        {
            var result = await _.SendAsync(new ProjectList.Query());
            
            result.Results.ShouldCount(5); // art + food projects
        }
        
        [Test]
        public async Task Can_list_by_closed_projects()
        {
            var result = await _.SendAsync(new ProjectList.Query { Closed = true });
            
            result.Results.ShouldMatchIds(_closedProject);
        }
        
        [Test]
        public async Task Can_list_by_category()
        {
            var result = await _.SendAsync(new ProjectList.Query { Category = _art.Name });
            
            result.Results.ShouldMatchIds(_artProjects);
        }
        
        [Test]
        public async Task Can_list_by_name()
        {
            var result = await _.SendAsync(new ProjectList.Query { Search = _artProjects.At(0).Name });
            
            result.Results.ShouldMatchIds(_artProjects.At(0));
        }
    }
}
