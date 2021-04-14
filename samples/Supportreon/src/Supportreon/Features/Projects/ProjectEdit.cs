using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Databases.EntityFramework;
using Miru.Domain;
using Miru.Mvc;
using Supportreon.Database;
using Supportreon.Domain;

namespace Supportreon.Features.Projects
{
    public class ProjectEdit
    {
        public class Query : IRequest<Command>
        {
            public long Id { get; set; }
        }
        
        public class Command : IRequest<Result>
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal MinimumDonation { get; set; }
            public long CategoryId { get; set; }
            public decimal? Goal { get; set; }
            public DateTime? EndDate { get; set; }
            
            public IEnumerable<ILookupable> Categories { get; set; }
        }

        public class Result
        {
            public Project Project { get; set; }
        }

        public class Handler : 
            IRequestHandler<Query, Command>, 
            IRequestHandler<Command, Result>
        {
            private readonly SupportreonDbContext _db;
            
            public Handler(SupportreonDbContext db)
            {
                _db = db;
            }
            
            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                var project = await _db.Projects.ByIdOrFailAsync(request.Id, ct);
                var categories = await _db.Categories.ToLookupableAsync(ct);
                
                return new Command
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Goal = project.Goal,
                    CategoryId = project.CategoryId,
                    EndDate = project.EndDate,
                    MinimumDonation = project.MinimumDonation,
                    Categories = categories
                };
            }
            
            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                if (await _db.Projects.UniqueAsync(request.Id, m => m.Name == request.Name, ct) == false)
                    throw new DomainException("Name is already in use. It should be unique");
                
                var project = await _db.Projects.ByIdAsync(request.Id) ?? new Project();

                project.Name = request.Name;
                project.Description = request.Description;
                project.MinimumDonation = request.MinimumDonation;
                project.CategoryId = request.CategoryId;
                project.Goal = request.Goal;
                project.EndDate = request.EndDate;

                await _db.SaveOrUpdate(project, ct);

                return new Result { Project = project };
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Name).NotEmpty().MaximumLength(64);
                    
                RuleFor(m => m.Description).NotEmpty().MaximumLength(1000);

                RuleFor(m => m.MinimumDonation).NotEmpty().GreaterThanOrEqualTo(Donation.Minimum);
                
                RuleFor(m => m.CategoryId).NotEmpty();
            }
        }
        
        public class ProjectsController : MiruController
        {
            [Route("/Projects/{id:long}/Edit")]
            public async Task<Command> Edit(Query query) => await SendAsync(query);

            [HttpPost, Route("/Projects/Save")]
            public async Task<Result> Save(Command command) => await SendAsync(command);
        }
    }
}
