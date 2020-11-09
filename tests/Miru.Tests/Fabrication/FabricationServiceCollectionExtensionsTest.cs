using System;
using System.Collections.Generic;
using AutoFixture;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Miru.Fabrication;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Fabrication
{
    public class FabricationServiceCollectionExtensionsTest
    {
        private IServiceProvider _sp;

        [OneTimeSetUp]
        public void Setup()
        {
            var services = new ServiceCollection()
                .AddFabrication<ThisFabricator>();
            
            _sp = services.BuildServiceProvider();
        }
        
        [Test]
        public void Fabricator_should_be_singleton()
        {
            var fabricator = _sp.GetService<Fabricator>();

            var otherFabricatorInstance = _sp.GetService<Fabricator>();
            
            fabricator.ShouldBe(otherFabricatorInstance);
        }
        
        [Test]
        public void Fabricator_should_be_the_same_type_used_in_add_fabrication()
        {
            var fabricator = _sp.GetService<Fabricator>();

            fabricator.ShouldBeOfType<ThisFabricator>();
        }
        
        [Test]
        public void Property_access_for_custom_fabricator_should_be_singleton()
        {
            var fabricator = _sp.GetService<ThisFabricator>();

            fabricator.Products.ShouldBe(fabricator.Products);
            
            fabricator.Products.ShouldBe(fabricator.Products);
        }
        
        public class ThisFabricator : Fabricator
        {
            public ThisFabricator(FabSupport context) : base(context)
            {
            }
        
            public ProductFabricator Products => FabFor<Product, ProductFabricator>();
        }

        public class ProductFabricator : CustomFabricator<Product, ProductFabricator>
        {
            public ProductFabricator(FabSupport fabricator) : base(fabricator) { }
        }
    
        public class Product { }
    }
}