using System.Collections.Generic;
using MediatR;

namespace Miru.Tests.Html.TagHelpers;

public class TeamList
{
    public class Query : IRequest<Result>
    {
    }

    public class Result
    {
        public IReadOnlyList<TeamView> Teams { get; set; } = new List<TeamView>();
    }

    public class TeamView
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}