using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru.Mvc;
using SelfImprov.Database;
using SelfImprov.Domain;

namespace SelfImprov.Features.Iterations
{
    public class IterationReview
    {
        public class Query : IRequest<Command>
        {
        }
        
        public class Command : IRequest<Iteration>
        {
            public IReadOnlyList<AreaReview> Areas { get; set; }  
        }

        public class AreaReview
        {
            public string Name { get; set; }
            public IList<GoalReview> Goals { get; set; }
            public long Id { get; set; }
        }

        public class GoalReview
        {
            public string Name { get; set; }
            public long Id { get; set; }
            public bool Achieved { get; set; }
        }

        public class Handler : 
            IRequestHandler<Query, Command>,
            IRequestHandler<Command, Iteration>
        {
            private readonly SelfImprovDbContext _db;

            public Handler(SelfImprovDbContext db)
            {
                _db = db;
            }

            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                return new Command
                {
                    Areas = await _db.Areas
                        .Select(x => new AreaReview
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Goals = x.Goals.Where(g => g.IsInactive == false).Select(g => new GoalReview
                            {
                                Name = g.Name,
                                Id = g.Id
                            }).ToList()
                        }).ToListAsync(ct)

                    //.ProjectToList<Area>()
                };
            }
            
            public async Task<Iteration> Handle(Command command, CancellationToken ct)
            {
                var iteration = new Iteration();
                var lastIteration = _db.Iterations.OrderByDescending(i => i.Number).FirstOrDefault();
                
                iteration.Number = lastIteration?.Number + 1 ?? 1;

                foreach (var goal in command.Areas.SelectMany(c => c.Goals))
                {
                    iteration.Achievements.Add(new Achievement
                    {
                        GoalId = goal.Id,
                        Achieved = goal.Achieved,
                        Iteration = iteration,
                    });
                }

                await _db.Iterations.AddAsync(iteration, ct);

                return iteration;
            }
        }

        public class Validation : AbstractValidator<Command>
        {
            public Validation()
            {
                RuleFor(m => m.Areas).NotEmpty();
            }
        }

        public class IterationsController : MiruController
        {
            [Route("/Iterations/Review")]
            public async Task<Command> Review(Query query) => await Send(query);

            [HttpPost, Route("/Iterations/Review")]
            public async Task<Iteration> Review(Command command) => await Send(command);
        }
    }
}
