using System.Linq;
using Microsoft.EntityFrameworkCore;
using MiruNext.Database;

namespace MiruNext.Features.Todos;

public class TodoList
{
    public class Query
    {
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

    [HttpGet("/Todos")]
    public class List : Endpoint2<Query>
    {
        private readonly AppDbContext _db;

        public List(AppDbContext db)
        {
            _db = db;
        }

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
    }
}