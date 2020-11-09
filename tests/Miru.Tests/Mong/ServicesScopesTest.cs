using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Miru.Settings;
using Miru.Testing;
using Mong.Database;
using Mong.Features.Accounts;
using Mong.Tests.Config;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Mong
{
    public class ServicesScopesTest
    {
        [Test]
        public void Services_passed_to_validators_should_be_the_same_to_handlers()
        {
            var testHost = new TestMiruHost();
            var app = testHost.Start(new TestsConfig(), services =>
            {
                services.AddSingleton(new DatabaseOptions { ConnectionString = "DataSource={{ db_dir }}Mong_test.db" });
            });

            MongDbContext firstScopeDb1;
            IValidator<AccountRegister.Command> firstScopeValidator1;
            
            using (var scope = app.WithScope())
            {
                firstScopeDb1 = scope.Get<MongDbContext>();
                var firstScopeDb2 = scope.Get<MongDbContext>();
                
                var validatorType = typeof(IValidator<>).MakeGenericType(typeof(AccountRegister.Command));

                firstScopeValidator1 = scope.Get(validatorType) as IValidator<AccountRegister.Command>;
                var firstScopeValidator2 = scope.Get(validatorType) as IValidator<AccountRegister.Command>;
                
                firstScopeDb1.ShouldBe(firstScopeDb2);
                firstScopeValidator1.ShouldNotBeNull();
                firstScopeValidator1.ShouldBe(firstScopeValidator2);                
            }

            Should.Throw<ValidationException>(() => app.SendSync(new AccountRegister.Command()));
            
            using (var scope = app.WithScope())
            {
                var secondScopeDb = scope.Get<MongDbContext>();
                
                var validatorType = typeof(IValidator<>).MakeGenericType(typeof(AccountRegister.Command));
                var secondScopeValidator = scope.Get(validatorType) as IValidator<AccountRegister.Command>;
                
                firstScopeDb1.ShouldNotBe(secondScopeDb);
                secondScopeValidator.ShouldNotBeNull();
                firstScopeValidator1.ShouldNotBe(secondScopeValidator);                
            }
            
            app.Dispose();
        }
    }
}