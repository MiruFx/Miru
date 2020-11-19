using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Miru.Security;
using Miru.Testing;
using Miru.Testing.Userfy;
using Mong.Domain;
using Mong.Features.Admin;
using NUnit.Framework;
using Shouldly;

namespace Mong.Tests.Features.Admin
{
    public class AdminIndexTest : OneCaseFeatureTest, IRequiresAuthenticatedAdmin
    {
        private IEnumerable<Topup> _lastMonth;
        private IEnumerable<Topup> _thisMonth;
        private IEnumerable<Topup> _lastWeek;
        private IEnumerable<Topup> _thisWeek;
        private AdminIndex.Result _result;

        public override async Task GivenAsync()
        {
            // _.MakeSavingLogin(_.Fab().Users.Admin().Make());
                
            _lastMonth = _.MakeMany<Topup>(5, m => m.PaidAt = DateTime.Now.AddDays(-50));
            // this month
            _thisMonth = _.MakeMany<Topup>(3, m => m.PaidAt = DateTime.Now.AddDays(-20));
            _lastWeek = _.MakeMany<Topup>(3, m => m.PaidAt = DateTime.Now.AddDays(-10));
            _thisWeek = _.MakeMany<Topup>(2, m => m.PaidAt = DateTime.Now);
            
            await _.SaveAsync(_lastMonth, _thisMonth, _lastWeek, _thisWeek);
            
            _result = await _.SendAsync(new AdminIndex.Query());
        }

        [Test]
        public void Total_topups_this_week()
        {
            _result.TotalThisWeek.ShouldBe(_thisWeek.Count());
        }
    }
}