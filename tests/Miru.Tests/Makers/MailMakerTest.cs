using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers
{
    public class MailMakerTest
    {
        private MiruPath _solutionDir;

        [SetUp]
        [TearDown]
        public void Setup()
        {
            _solutionDir = A.TempPath("Miru", "Shopifu");
            
            Directories.DeleteIfExists(_solutionDir);
        }
        
        [Test]
        public void Make_mailable_and_view()
        {
            // arrange
            var m = new Maker(new MiruSolution(_solutionDir));
            
            // act
            m.Mail("Accounts", "Welcome");
            
            // assert
            (m.Solution.FeaturesDir / "Accounts" / "WelcomeMail.cs")
                .ShouldContain(
                    "namespace Shopifu.Features.Accounts",
                    "public class WelcomeMail : Mailable");
            
            (m.Solution.FeaturesDir / "Accounts" / "WelcomeMail.cshtml")
                .ShouldContain(
                    "@model Shopifu.Domain.User");
        }
    }
}