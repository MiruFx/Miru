using System.Threading;
using System.Threading.Tasks;
using Corpo.Skeleton.Database;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mvc;

namespace Corpo.Skeleton.Features.Teams;

public class TeamDelete
{
    public class Command : IRequest<FeatureResult>
    {
        public long Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, FeatureResult>
    {
        private readonly SkeletonDbContext _db;

        public Handler(SkeletonDbContext db)
        {
            _db = db;
        }
            
        public async Task<FeatureResult> Handle(Command request, CancellationToken ct)
        {
            var team = await _db.Teams.ByIdOrFailAsync(request.Id, ct);

            _db.Teams.Remove(team);
                
            return new FeatureResult<TeamList>()
                .Success($"Team {0} has been deleted");
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
        }
    }
        
    public class GroupsController : MiruController
    {
        [HttpPost("/Teams/Delete/{Id}")]
        public async Task<FeatureResult> Delete(Command command) => await SendAsync(command);
    }
}