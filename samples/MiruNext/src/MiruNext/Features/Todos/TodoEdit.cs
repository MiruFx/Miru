using System.Linq;
using Microsoft.EntityFrameworkCore;
using MiruNext.Database;

namespace MiruNext.Features.Todos;

public class TodoEdit
{
    public class Query
    {
    }
    
    public class Command
    {
        public long TodoId { get; set; }
        public string Name { get; set; }
    }

    public class Result
    {
        public TodoView[] Todos { get; set; } = Array.Empty<TodoView>();
    }

    public class TodoView
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    [HttpGet("/todos/{TodoId}")]
    public class Edit : Endpoint2<Query>
    {
        private readonly AppDbContext _db;

        public Edit(AppDbContext db) => _db = db;

        public override async Task HandleAsync(Query request, CancellationToken ct)
        {
            var result = new Result
            {
                Todos = await _db.Todos
                    .Select(x => new TodoView
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToArrayAsync(ct)
            };

            await RespondAsync(result);
        }
        
        // public override async Task HandleAsync(Command request, CancellationToken ct)
        // {
        //     var result = new Result
        //     {
        //         Todos = await _db.Todos
        //             .Select(x => new TodoView
        //             {
        //                 Id = x.Id,
        //                 Name = x.Name
        //             })
        //             .ToArrayAsync(ct)
        //     };
        //
        //     await RespondAsync(result);
        // }
    }
    
    [HttpGet("/todos/new")]
    public class New : Endpoint2<Query>
    {
        // private readonly AppDbContext _db;
        //
        // public New(AppDbContext db) => _db = db;

        public override async Task HandleAsync(Query request, CancellationToken ct)
        {
            await RespondAsync(new Command());
        }
    }
}