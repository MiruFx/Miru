using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Miru.Core;
using Miru.Mvc;
using Miru.Storages;

namespace Playground.Features.Uploads
{
    public class UploadEdit
    {
        public class Query : IRequest<Command>
        {
        }

        public class Command : IRequest<Result>
        {
            public IFormFile OneFile { get; set; }
            public IFormFile NotEmptyFile { get; set; }
            public IFormFile OnlyTextFile { get; set; }
        }

        public class Result
        {
            public string OneFilePath { get; set; }
            public string NotEmptyFilePath { get; set; }
            public string OnlyTextFilePath { get; set; }
        }
        
        public class Handler :
            IRequestHandler<Command, Result>,
            IRequestHandler<Query, Command>
        {
            private readonly IStorage _storage;

            public Handler(IStorage storage)
            {
                _storage = storage;
            }

            public Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Command());
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var oneFilePath = A.Path / "uploads" / request.OneFile.FileName;
                
                await _storage.PutAsync(oneFilePath, request.OneFile.OpenReadStream());

                return new Result
                {
                    OneFilePath = oneFilePath
                };
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.OneFile).NotEmpty();
                
                RuleFor(x => x.NotEmptyFile.Length).NotEmpty().When(x => x.NotEmptyFile != null);

                RuleFor(x => x.OnlyTextFile.ContentType)
                    .NotEmpty()
                    .Must(x => x.Equals("text/text") || x.Equals("text/plain"))
                    .WithMessage("File type is not supported")
                    .When(x => x.OnlyTextFile != null);
            }
        }

        public class UploadsController : MiruController
        {
            [HttpGet("/Uploads")]
            public async Task<Command> Edit(Query query) => await SendAsync(query);

            [HttpPost("/Uploads")]
            public async Task<Result> Edit(Command command) => await SendAsync(command);
        }
    }
}