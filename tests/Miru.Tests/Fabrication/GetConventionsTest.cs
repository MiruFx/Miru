using System;
using System.Collections.Generic;
using AutoFixture;
using Bogus;
using Miru.Fabrication.FixtureConventions;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Fabrication
{
    public class GetConventionsTest
    {
        [Test]
        public void Get_convention_for_property()
        {
            var fixture = CreateFixture(_ =>
            {
                _.IfPropertyTypeIs<int>().Use(10);
            });

            fixture.WhatConventionsDoIHave().ShouldContain(
                "- IfProperty p => (p.PropertyType == System.Int32) => Use");
        }
        
        [Test]
        public void Get_convention_for_class()
        {
            var fixture = CreateFixture(_ =>
            {
                _.IfClassIs<Customer>().Use(new Customer());
            });

            fixture.WhatConventionsDoIHave().ShouldContain(
                "- IfClass t => (t == Miru.Tests.Fabrication.GetConventionsTest+Customer) => Use");
        }
        
        [Test]
        public void Get_convention_for_ignore()
        {
            var fixture = CreateFixture(_ =>
            {
                _.IfProperty(p => p.PropertyType.ImplementsEnumerableOf<Order>()).Ignore();
            });

            fixture.WhatConventionsDoIHave().ShouldContain(
                "- IfProperty p => p.PropertyType.ImplementsEnumerableOf() => Ignore");
        }

        [Test]
        public void Get_filter_with_no_action()
        {
            var fixture = CreateFixture(_ =>
            {
                _.IfProperty(p => p.PropertyType.ImplementsEnumerableOf<Order>());
            });

            fixture.WhatConventionsDoIHave().ShouldContain(
                "- IfProperty p => p.PropertyType.ImplementsEnumerableOf() => NO ACTION DEFINED!");
        }
        
        [Test]
        public void Get_convention_with_different_add_convention_calls()
        {
            var faker = new Faker();
            
            var fixture = new Fixture()
                .AddConvention(faker, _ =>
                {
                    _.IfPropertyNameIs("FirstName").Use("First one");
                })
                .AddConvention(faker, _ =>
                {
                    _.IfPropertyNameIs("FirstName").Use("Last one");
                });
        }
        
        [Test]
        public void Dump_all_conventions()
        {
            var fixture = CreateFixture(_ =>
            {
                _.IfPropertyTypeIs<int>().Ignore();
                _.IfPropertyNameIs("FullName").Use("Joao Silva");
                _.IfPropertyNameStarts("Country").Use(f => f.Address.Country());
            }) 
            .AddConvention(new Faker(), _ =>
            {
                _.IfProperty(p => p.Name == "FirstName").Use("Appear first");
            });;

            var conventions = fixture.WhatConventionsDoIHave();
            
            Console.WriteLine(conventions);
        }
        
        private Fixture CreateFixture(Action<ConventionExpression> cfg) => 
            new Fixture().AddConvention(new Faker(), cfg);

        public interface IEntity
        {
        }
        
        public class Customer : IEntity
        {
            public string FirstName { get; set; }
            public IEnumerable<Order> Orders { get; set; }
        }
        
        public class Order : IEntity
        {
            public long Id { get; }
        }
    }
}