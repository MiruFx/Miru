using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Mvc;

namespace Playground.Features.Errors;

public class ErrorNew
{
    public class Query : IRequest<Command>
    {
    }
    
    public class Command : IRequest<Result>
    {
    }

    public class Result
    {
    }
        
    public class Handler : 
        IRequestHandler<Query, Command>,
        IRequestHandler<Command, Result>
    {
        public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return new Command();
        } 
        
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            
            throw new Exception("Exception thrown from Command Handler");
        }
    }
    
    public class ErrorsController : MiruController
    {
        [HttpGet("/Errors/New")]
        public async Task<Command> New(Query query) => await SendAsync(query);

        [HttpPost("/Errors/New")]
        public async Task<Result> New(Command command) => await SendAsync(command);
    }
}