using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers
{
    public class EmailMakerTest
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
        public void Make_a_email()
        {
            // arrange
            var m = new Maker(new MiruSolution(_solutionDir));
            
            // act
            m.Email("Users", "User", "Welcome");
            
            // assert
            (m.Solution.FeaturesDir / "Users" / "UserWelcomeMail.cs")
                .ShouldExistAndContains(
                    "namespace Shopifu.Features.Users",
                    "public class UserWelcomeMail : Mailable");
            
            (m.Solution.FeaturesDir / "Users" / "UserWelcomeMail.cshtml")
                .ShouldExistAndContains(
                    "@model Shopifu.Domain.User",
                    "Content in markdown, @Model.Name!");
        }
    }
}