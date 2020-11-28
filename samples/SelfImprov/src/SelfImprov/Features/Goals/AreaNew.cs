using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru;
using Miru.Domain;
using Miru.Mvc;
using SelfImprov.Database;
using SelfImprov.Domain;

namespace SelfImprov.Features.Goals
{
    public class AreaNew
    {
        public class Command : IRequest<Area>
        {
            public string Name { get; set; }
        }
        
        public class Handler : IRequestHandler<Command, Area>
        {
            private readonly SelfImprovDbContext _db;
            private readonly IMapper _mapper;

            public Handler(SelfImprovDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Area> Handle(Command request, CancellationToken ct)
            {
                // TODO: unique per user
                if (await _db.Areas.AnyAsync(x => x.Name == request.Name, ct))
                    throw new DomainException("Name is already in use. It should be unique");
                
                var area = _mapper.Map<Area>(request);
                
                await _db.Areas.AddAsync(area, ct);
                
                return area;
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }
        
        public class AreasController : MiruController
        {
            [Route("/Areas/New")]
            public Command AreaNew() => new Command();
            
            [HttpPost, Route("/Areas/New")]
            public async Task<Area> AreaNew(Command request) => await SendAsync(request);
        }
        
        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<Command, Area>();
            }
        }
    }
}