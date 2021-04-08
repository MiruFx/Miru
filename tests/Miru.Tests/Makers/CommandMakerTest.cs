using System;
using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;
using Serilog;

namespace Miru.Tests.Makers
{
    public class CommandMakerTest
    {
        private MiruPath _solutionDir;

        [SetUp]
        [TearDown]
        public void Setup()
        {
            _solutionDir = A.TempPath("Miru", "Shopifu");

            Console.WriteLine(_solutionDir);
            
            Directories.DeleteIfExists(_solutionDir);
        }
        
        [Test]
        public void Make_a_command()
        {
            // arrange
            var m = new Maker(new MiruSolution(_solutionDir));
            
            // act
            m.Command("Carts", "Cart", "New");
            
            // assert
            (m.Solution.FeaturesDir / "Carts" / "CartNew.cs")
                .ShouldContain(
                    "namespace Shopifu.Features.Carts",
                    "public class CartNew");
            
            (m.Solution.FeaturesDir / "Carts" / "New.cshtml")
                .ShouldContain(
                    "@model CartNew.Command");
            
            (m.Solution.FeaturesDir / "Carts" / "_New.turbo.cshtml").ShouldExist();
            
            (m.Solution.AppTestsDir / "Features" / "Carts" / "CartNewTest.cs").ShouldExist();
            
            (m.Solution.AppPageTestsDir / "Pages" / "Carts" / "CartNewPageTest.cs")
                .ShouldContain(
                    "namespace Shopifu.PageTests.Pages.Carts",
                    "public class CartNewPageTest : PageTest");
        }
        
        [Test]
        public void Make_a_command_in_sub_folders()
        {
            // arrange
            var m = new Maker(new MiruSolution(_solutionDir));
            
            // act
            m.Command("Admin/Catalogue/Products", "Product", "New");
            
            // assert
            (m.Solution.FeaturesDir / "Admin" / "Catalogue" / "Products" / "ProductNew.cs")
                .ShouldContain(
                    "namespace Shopifu.Features.Admin.Catalogue.Products",
                    "public class ProductNew");
        }
    }
}