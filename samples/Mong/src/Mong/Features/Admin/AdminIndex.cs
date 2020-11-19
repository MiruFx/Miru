using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Baseline.Dates;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Miru;
using Miru.Mvc;
using Mong.Database;

namespace Mong.Features.Admin
{
    public class AdminIndex
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public int TotalThisWeek { get; set; }
            public int TotalLastWeek { get; set; }
            public int TotalThisMonth { get; set; }
            public int TotalLastMonth { get; set; }
        }

        public class CountPerDate
        {
            public DateTime PaidAt { get; set; }
            public int Total { get; set; }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly MongDbContext _db;

            public Handler(MongDbContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var sql = @"
select
     count(PaidAt) Total,
     strftime('%Y-%m-%d', PaidAt) PaidAt
from
     Topups
WHERE 
	PaidAt > date('now','-2 months')
group BY
	strftime('%Y-%M-%d', PaidAt)
";

                var countPerDates = await _db.Database.GetDbConnection().QueryAsync<CountPerDate>(sql);

                var result = new Result
                {
                    TotalThisWeek = countPerDates
                        .Where(m => m.PaidAt >= 1.Weeks().Ago()).Sum(m => m.Total),
                    
                    TotalLastWeek = countPerDates
                        .Where(m => m.PaidAt.Between(2.Weeks().Ago(), 1.Weeks().Ago())).Sum(m => m.Total),
                    
                    TotalThisMonth = countPerDates
                        .Where(m => m.PaidAt >= 1.Months().Ago()).Sum(m => m.Total),
                    
                    TotalLastMonth = countPerDates
                        .Where(m => m.PaidAt.Between(2.Months().Ago(), 1.Months().Ago())).Sum(m => m.Total)
                };

                return result;
            }
        }
        
        public class AdminController : MiruController
        {
            public async Task<Result> Index(Query query) => await SendAsync(query);
        }
    }
}